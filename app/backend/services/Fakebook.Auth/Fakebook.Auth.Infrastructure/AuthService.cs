using Fakebook.Auth.Application.Auth;
using Fakebook.Auth.Domain.Entities;
using Fakebook.Auth.Infrastructure.Persistence;
using Fakebook.Auth.Infrastructure.Security;
using Fakebook.BuildingBlocks.Application.Abstractions.Clock;
using Fakebook.BuildingBlocks.Application.Common.Errors;
using Fakebook.BuildingBlocks.Application.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Auth.Infrastructure;

public sealed class AuthService : IAuthService
{
    private readonly AuthDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly PasswordService _passwordService;
    private readonly TokenService _tokenService;

    public AuthService(
        AuthDbContext dbContext,
        IDateTimeProvider dateTimeProvider,
        PasswordService passwordService,
        TokenService tokenService)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var email = NormalizeEmail(request.Email);
        var userName = NormalizeUserName(request.UserName);

        var userExists = await _dbContext.Users
            .AnyAsync(user => user.Email == email || user.UserName == userName, cancellationToken);

        if (userExists)
        {
            return Result<AuthResponse>.Failure(
                new Error("user_already_exists", "Email or userName already exists."));
        }

        var passwordHash = _passwordService.Hash(request.Password);
        var newUser = CreateUser(email, userName, passwordHash);

        _dbContext.Users.Add(newUser);

        var response = CreateAuthResponse(newUser);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(response);
    }

    public async Task<Result<AuthResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var login = NormalizeLogin(request.EmailOrUserName);

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(
                user => user.Email == login || user.UserName == login,
                cancellationToken);

        if (user is null || !user.IsActive)
        {
            return Result<AuthResponse>.Failure(
                new Error("invalid_credentials", "Invalid email/userName or password."));
        }

        var isPasswordValid = _passwordService.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return Result<AuthResponse>.Failure(
                new Error("invalid_credentials", "Invalid email/userName or password."));
        }

        var response = CreateAuthResponse(user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(response);
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var tokenHash = _tokenService.HashRefreshToken(request.RefreshToken);

        var storedToken = await _dbContext.RefreshTokens
            .Include(token => token.User)
            .FirstOrDefaultAsync(token => token.TokenHash == tokenHash, cancellationToken);

        var utcNow = new DateTimeOffset(_dateTimeProvider.UtcNow, TimeSpan.Zero);

        if (storedToken is null || !IsRefreshTokenActive(storedToken, utcNow) || !storedToken.User.IsActive)
        {
            return Result<AuthResponse>.Failure(
                new Error("invalid_refresh_token", "Refresh token is invalid or expired."));
        }

        RevokeRefreshToken(storedToken, utcNow);

        var response = CreateAuthResponse(storedToken.User);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(response);
    }

    public async Task<Result<bool>> LogoutAsync(
        LogoutRequest request,
        CancellationToken cancellationToken)
    {
        var tokenHash = _tokenService.HashRefreshToken(request.RefreshToken);

        var storedToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(token => token.TokenHash == tokenHash, cancellationToken);

        if (storedToken is not null)
        {
            var utcNow = new DateTimeOffset(_dateTimeProvider.UtcNow, TimeSpan.Zero);

            RevokeRefreshToken(storedToken, utcNow);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return Result<bool>.Success(true);
    }

    public async Task<Result<CurrentUserResponse>> GetCurrentUserAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

        if (user is null || !user.IsActive)
        {
            return Result<CurrentUserResponse>.Failure(
                new Error("user_not_found", "Current user was not found."));
        }

        var response = new CurrentUserResponse(
            user.Id,
            user.Email,
            user.UserName);

        return Result<CurrentUserResponse>.Success(response);
    }

    #region Private

    private AuthResponse CreateAuthResponse(User user)
    {
        var accessToken = _tokenService.CreateAccessToken(user);
        var refreshToken = _tokenService.CreateRefreshToken();

        _dbContext.RefreshTokens.Add(
            CreateRefreshToken(
                user.Id,
                refreshToken.TokenHash,
                refreshToken.ExpiresAtUtc,
                new DateTimeOffset(_dateTimeProvider.UtcNow, TimeSpan.Zero)));

        return new AuthResponse(
            user.Id,
            user.Email,
            user.UserName,
            accessToken.Token,
            accessToken.ExpiresAtUtc,
            refreshToken.PlainToken,
            refreshToken.ExpiresAtUtc);
    }

    private User CreateUser(string email, string userName, string passwordHash)
    {
        var utcNow = _dateTimeProvider.UtcNow;

        return new User
        {
            Email = email,
            UserName = userName,
            PasswordHash = passwordHash,
            CreatedAtUtc = utcNow,
            UpdatedAtUtc = utcNow,
            IsActive = true
        };
    }

    private static RefreshToken CreateRefreshToken(
        Guid userId,
        string tokenHash,
        DateTimeOffset expiresAtUtc,
        DateTimeOffset createdAtUtc)
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