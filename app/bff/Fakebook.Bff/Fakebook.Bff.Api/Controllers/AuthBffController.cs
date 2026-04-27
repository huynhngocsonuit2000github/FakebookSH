using Fakebook.Bff.Api.Downstreams.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.Bff.Api.Controllers;

[ApiController]
[Route("api/bff/auth")]
public sealed class AuthBffController : BffControllerBase
{
    private readonly IAuthApiClient _authApiClient;

    public AuthBffController(IAuthApiClient authApiClient)
    {
        _authApiClient = authApiClient;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    public Task<IActionResult> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(
            () => _authApiClient.RegisterAsync(request, cancellationToken));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    public Task<IActionResult> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(
            () => _authApiClient.LoginAsync(request, cancellationToken));
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    public Task<IActionResult> RefreshToken(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(
            () => _authApiClient.RefreshTokenAsync(request, cancellationToken));
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> Logout(
        LogoutRequest request,
        CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(
            async () =>
            {
                await _authApiClient.LogoutAsync(request, cancellationToken);
                return NoContent();
            });
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType<CurrentUserResponse>(StatusCodes.Status200OK)]
    public Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(
            () => _authApiClient.GetCurrentUserAsync(cancellationToken));
    }
}
