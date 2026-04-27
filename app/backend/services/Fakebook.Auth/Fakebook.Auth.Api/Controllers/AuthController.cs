using Fakebook.Auth.Application.Auth;
using Fakebook.BuildingBlocks.Api.Controllers;
using Fakebook.BuildingBlocks.Application.Messaging;
using Fakebook.BuildingBlocks.Messaging.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fakebook.Auth.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ApiControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMessagePublisher _messagePublisher;

    public AuthController(IAuthService authService, IServiceProvider serviceProvider, IMessagePublisher messagePublisher) : base(serviceProvider)
    {
        _authService = authService;
        _messagePublisher = messagePublisher;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        return ExecuteAndValidatorAsync(request, () => _authService.RegisterAsync(request, cancellationToken));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        return ExecuteAndValidatorAsync(request, () => _authService.LoginAsync(request, cancellationToken));
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public Task<IActionResult> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        return ExecuteAndValidatorAsync(request, () => _authService.RefreshTokenAsync(request, cancellationToken));
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout(LogoutRequest request, CancellationToken cancellationToken)
    {
        var result = await ExecuteAndValidatorAsync(request, () => _authService.LogoutAsync(request, cancellationToken));

        return result is OkObjectResult ? NoContent() : result;
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        await _messagePublisher.PublishAsync(new UserRegisteredIntegrationEvent(Guid.NewGuid(), "email", "username", DateTime.UtcNow), cancellationToken);

        return Ok(1);

        var userIdText = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdText, out var userId))
        {
            return Unauthorized(new { message = "Invalid access token." });
        }

        var result = await _authService.GetCurrentUserAsync(userId, cancellationToken);
        return ToActionResult(result);
    }
}
