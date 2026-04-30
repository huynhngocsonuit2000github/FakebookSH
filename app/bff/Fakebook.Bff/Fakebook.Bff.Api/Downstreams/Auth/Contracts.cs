namespace Fakebook.Bff.Api.Downstreams.Auth;

public sealed record RegisterRequest(string FirstName, string LastName, string Email, string UserName, string Password);

public sealed record LoginRequest(string EmailOrUserName, string Password);

public sealed record RefreshTokenRequest(string RefreshToken);

public sealed record LogoutRequest(string RefreshToken);

public sealed record AuthResponse(Guid UserId, string Email, string UserName, string FirstName, string LastName, string AccessToken, DateTimeOffset AccessTokenExpiresAtUtc, string RefreshToken, DateTimeOffset RefreshTokenExpiresAtUtc);

public sealed record CurrentUserResponse(Guid UserId, string Email, string UserName, string FirstName, string LastName);
