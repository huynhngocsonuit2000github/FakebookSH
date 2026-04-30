using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Fakebook.Auth.Application.Boundary.Security;
using Fakebook.Auth.Domain.Entities;
using Fakebook.BuildingBlocks.Application.Abstractions.Clock;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Fakebook.Auth.Infrastructure.Security;

public sealed class TokenService : ITokenService
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtOptions _options;

    public TokenService(IDateTimeProvider dateTimeProvider, IOptions<JwtOptions> options)
    {
        _dateTimeProvider = dateTimeProvider;
        _options = options.Value;
    }

    public (string Token, DateTimeOffset ExpiresAtUtc) CreateAccessToken(User user)
    {
        var utcNow = new DateTimeOffset(_dateTimeProvider.UtcNow, TimeSpan.Zero);
        var expiresAtUtc = utcNow.AddMinutes(_options.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new("user_name", user.UserName),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: _options.Issuer, audience: _options.Audience, claims: claims, expires: expiresAtUtc.UtcDateTime, signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
    }

    public (string PlainToken, string TokenHash, DateTimeOffset ExpiresAtUtc) CreateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var plainToken = Convert.ToBase64String(bytes);
        var tokenHash = HashRefreshToken(plainToken);
        var utcNow = new DateTimeOffset(_dateTimeProvider.UtcNow, TimeSpan.Zero);
        var expiresAtUtc = utcNow.AddDays(_options.RefreshTokenDays);

        return (plainToken, tokenHash, expiresAtUtc);
    }

    public string HashRefreshToken(string refreshToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
