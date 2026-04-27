using Fakebook.Bff.Api.Downstreams.Cores;

namespace Fakebook.Bff.Api.Downstreams.Auth;

public sealed class AuthApiClient : HttpClientHandlerBase, IAuthApiClient
{
    public AuthApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<AuthResponse> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        return PostForJsonAsync<RegisterRequest, AuthResponse>(
            "api/auth/register",
            request,
            cancellationToken);
    }

    public Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        return PostForJsonAsync<LoginRequest, AuthResponse>(
            "api/auth/login",
            request,
            cancellationToken);
    }

    public Task<AuthResponse> RefreshTokenAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        return PostForJsonAsync<RefreshTokenRequest, AuthResponse>(
            "api/auth/refresh-token",
            request,
            cancellationToken);
    }

    public Task LogoutAsync(
        LogoutRequest request,
        CancellationToken cancellationToken)
    {
        return PostAsync(
            "api/auth/logout",
            request,
            cancellationToken);
    }

    public Task<CurrentUserResponse> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        return GetForJsonAsync<CurrentUserResponse>("api/auth/me", cancellationToken);
    }
}
