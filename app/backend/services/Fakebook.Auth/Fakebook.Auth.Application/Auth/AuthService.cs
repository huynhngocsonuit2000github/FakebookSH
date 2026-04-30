using Fakebook.Auth.Application.Boundary.Repositories;
using Fakebook.Auth.Application.Boundary.Security;
using Fakebook.Auth.Domain.Entities;
using Fakebook.BuildingBlocks.Application.Abstractions.Clock;
using Fakebook.BuildingBlocks.Application.Common.Errors;
using Fakebook.BuildingBlocks.Application.Common.Results;
using Fakebook.BuildingBlocks.Application.Messaging;
using Fakebook.BuildingBlocks.Messaging.Events;

namespace Fakebook.Auth.Application.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IMessagePublisher _messagePublisher;

    public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IDateTimeProvider dateTimeProvider, IPasswordService passwordService, ITokenService tokenService, IMessagePublisher messagePublisher)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _dateTimeProvider = dateTimeProvider;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _messagePublisher = messagePublisher;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var firstName = NormalizeName(request.FirstName);
        var lastName = NormalizeName(request.LastName);
        var email = NormalizeEmail(request.Email);
        var userName = NormalizeUserName(request.UserName);

        var userExists = await _userRepository.FirstOrDefaultAsync(user => user.Email == email || user.UserName == userName, cancellationToken);

        if (userExists is not null)
        {
            return Result<AuthResponse>.Failure(new Error("user_already_exists", "Email or userName already exists."));
        }

        var passwordHash = _passwordService.Hash(request.Password);
        var newUser = CreateUser(firstName, lastName, email, userName, passwordHash);

        await _userRepository.AddAsync(newUser, cancellationToken);

        var response = await CreateAuthResponseAsync(newUser);

        await _userRepository.SaveChangesAsync(cancellationToken);

        await _messagePublisher.PublishAsync(new UserRegisteredIntegrationEvent(newUser.Id, newUser.Email, newUser.UserName, newUser.FirstName, newUser.LastName, DateTime.UtcNow), cancellationToken);

        return Result<AuthResponse>.Success(response);
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var login = NormalizeLogin(request.EmailOrUserName);

        var user = await _userRepository.FirstOrDefaultAsync(user => user.Email == login || user.UserName == login, cancellationToken);

        if (user is null || !user.IsActive)
        {
            return Result<AuthResponse>.Failure(new Error("invalid_credentials", "Invalid email/userName or password."));
        }

        var isPasswordValid = _passwordService.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return Result<AuthResponse>.Failure(new Error("invalid_credentials", "Invalid email/userName or password."));
        }

        var response = await CreateAuthResponseAsync(user);

        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(response);
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var tokenHash = _tokenService.HashRefreshToken(request.RefreshToken);

        var storedToken = await _refreshTokenRepository.GetByTokenHashWithUserAsync(tokenHash, cancellationToken);

        var utcNow = new DateTimeOffset(_dateTimeProvider.UtcNow, TimeSpan.Zero);

        if (storedToken is null || !IsRefreshTokenActive(storedToken, utcNow) || !storedToken.User.IsActive)
        {
            return Result<AuthResponse>.Failure(new Error("invalid_refresh_token", "Refresh token is invalid or expired."));
        }

        RevokeRefreshToken(storedToken, utcNow);

        var response = await CreateAuthResponseAsync(storedToken.User);

        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(response);
    }

    public async Task<Result<bool>> LogoutAsync(LogoutRequest request, CancellationToken cancellationToken)
    {
        var tokenHash = _tokenService.HashRefreshToken(request.RefreshToken);

        var storedToken = await _refreshTokenRepository.FirstOrDefaultAsync(token => token.TokenHash == tokenHash, cancellationToken);

        if (storedToken is not null)
        {
            var utcNow = new DateTimeOffset(_dateTimeProvider.UtcNow, TimeSpan.Zero);

            RevokeRefreshToken(storedToken, utcNow);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
        }

        return Result<bool>.Success(true);
    }

    public async Task<Result<CurrentUserResponse>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null || !user.IsActive)
        {
            return Result<CurrentUserResponse>.Failure(new Error("user_not_found", "Current user was not found."));
        }

        var response = new CurrentUserResponse(user.Id, user.Email, user.UserName, user.FirstName, user.LastName);

        return Result<CurrentUserResponse>.Success(response);
    }

    #region Private

    private async Task<AuthResponse> CreateAuthResponseAsync(User user)
    {
        var accessToken = _tokenService.CreateAccessToken(user);
        var refreshToken = _tokenService.CreateRefreshToken();

        await _refreshTokenRepository.AddAsync(CreateRefreshToken(user.Id, refreshToken.TokenHash, refreshToken.ExpiresAtUtc, new DateTimeOffset(_dateTimeProvider.UtcNow, TimeSpan.Zero)), CancellationToken.None);

        return new AuthResponse(user.Id, user.Email, user.UserName, user.FirstName, user.LastName, accessToken.Token, accessToken.ExpiresAtUtc, refreshToken.PlainToken, refreshToken.ExpiresAtUtc);
    }

    private User CreateUser(string firstName, string lastName, string email, string userName, string passwordHash)
    {
        var utcNow = _dateTimeProvider.UtcNow;

        return new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = userName,
            PasswordHash = passwordHash,
            CreatedAtUtc = utcNow,
            UpdatedAtUtc = utcNow,
            IsActive = true
        };
    }

    private static RefreshToken CreateRefreshToken(Guid userId, string tokenHash, DateTimeOffset expiresAtUtc, DateTimeOffset createdAtUtc)
    {
        return new RefreshToken
        {
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAtUtc = expiresAtUtc,
            CreatedAtUtc = createdAtUtc
        };
    }

    private static bool IsRefreshTokenActive(RefreshToken refreshToken, DateTimeOffset utcNow)
    {
        return refreshToken.RevokedAtUtc is null && refreshToken.ExpiresAtUtc > utcNow;
    }

    private static void RevokeRefreshToken(RefreshToken refreshToken, DateTimeOffset revokedAtUtc)
    {
        refreshToken.RevokedAtUtc ??= revokedAtUtc;
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    private static string NormalizeName(string name)
    {
        return name.Trim();
    }

    private static string NormalizeUserName(string userName)
    {
        return userName.Trim().ToLowerInvariant();
    }

    private static string NormalizeLogin(string emailOrUserName)
    {
        return emailOrUserName.Trim().ToLowerInvariant();
    }

    #endregion Private
}
