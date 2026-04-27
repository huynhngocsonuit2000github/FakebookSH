namespace Fakebook.Auth.Application.Auth;

public sealed record RegisterRequest(string Email, string UserName, string Password);

public sealed record LoginRequest(string EmailOrUserName, string Password);

public sealed record RefreshTokenRequest(string RefreshToken);

public sealed record LogoutRequest(string RefreshToken);

public sealed record AuthResponse(Guid UserId, string Email, string UserName, string AccessToken, DateTimeOffset AccessTokenExpiresAtUtc, string RefreshToken, DateTimeOffset RefreshTokenExpiresAtUtc);

public sealed record CurrentUserResponse(Guid UserId, string Email, string UserName);
