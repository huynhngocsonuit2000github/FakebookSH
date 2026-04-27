using Fakebook.Auth.Domain.Entities;

namespace Fakebook.Auth.Application.Boundary.Security;

public interface ITokenService
{
    (string Token, DateTimeOffset ExpiresAtUtc) CreateAccessToken(User user);

    (string PlainToken, string TokenHash, DateTimeOffset ExpiresAtUtc) CreateRefreshToken();

    string HashRefreshToken(string refreshToken);
}