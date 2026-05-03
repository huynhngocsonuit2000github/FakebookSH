using Fakebook.Bff.Api.Downstreams.Auth;
using Fakebook.Bff.Api.Downstreams.Cores;
using Fakebook.Bff.Api.Downstreams.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace Fakebook.Bff.Api.Controllers;

[ApiController]
[Route("api/bff/auth")]
public sealed class AuthBffController : BffControllerBase
{
    private const string AccessTokenExpiresAtUtcAuthenticationTokenName = "access_token_expires_at_utc";
    private const string RefreshTokenAuthenticationTokenName = "refresh_token";
    private static readonly TimeSpan DownstreamTokenRefreshSkew = TimeSpan.FromSeconds(30);

    private readonly IAuthApiClient _authApiClient;

    public AuthBffController(IAuthApiClient authApiClient)
    {
        _authApiClient = authApiClient;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType<CurrentUserResponse>(StatusCodes.Status200OK)]
    public Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(async () =>
        {
            var response = await _authApiClient.RegisterAsync(request, cancellationToken);
            await SignInAsync(response);

            return Ok(ToCurrentUserResponse(response));
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType<CurrentUserResponse>(StatusCodes.Status200OK)]
    public Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(async () =>
        {
            var response = await _authApiClient.LoginAsync(request, cancellationToken);
            await SignInAsync(response);

            return Ok(ToCurrentUserResponse(response));
        });
    }

    [HttpPost("refresh-token")]
    [Authorize]
    [ProducesResponseType<CurrentUserResponse>(StatusCodes.Status200OK)]
    public Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(async () =>
        {
            var response = await RefreshSessionAsync(cancellationToken);

            if (response is null)
            {
                return Unauthorized(new { message = "Authentication session has expired." });
            }

            return Ok(ToCurrentUserResponse(response));
        });
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(async () =>
        {
            var refreshToken = await GetAuthenticationTokenAsync(RefreshTokenAuthenticationTokenName);

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                await _authApiClient.LogoutAsync(new LogoutRequest(refreshToken), cancellationToken);
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return NoContent();
        });
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType<CurrentUserResponse>(StatusCodes.Status200OK)]
    public Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(async () =>
        {
            var hasDownstreamToken = await EnsureDownstreamTokenAsync(cancellationToken);

            if (!hasDownstreamToken)
            {
                return Unauthorized(new { message = "Authentication session has expired." });
            }

            var currentUser = await _authApiClient.GetCurrentUserAsync(cancellationToken);
            return Ok(currentUser);
        });
    }

    private async Task SignInAsync(AuthResponse response)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, response.UserId.ToString()),
            new(ClaimTypes.Email, response.Email),
            new("user_name", response.UserName),
            new(ClaimTypes.GivenName, response.FirstName),
            new(ClaimTypes.Surname, response.LastName)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var properties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = response.RefreshTokenExpiresAtUtc,
            IsPersistent = true
        };

        properties.StoreTokens(new[]
        {
            new AuthenticationToken
            {
                Name = DownstreamRequestContextHandler.AccessTokenAuthenticationTokenName,
                Value = response.AccessToken
            },
            new AuthenticationToken
            {
                Name = AccessTokenExpiresAtUtcAuthenticationTokenName,
                Value = response.AccessTokenExpiresAtUtc.ToString("O", CultureInfo.InvariantCulture)
            },
            new AuthenticationToken
            {
                Name = RefreshTokenAuthenticationTokenName,
                Value = response.RefreshToken
            }
        });

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

        HttpContext.User = principal;
        HttpContext.Items[DownstreamRequestContextHandler.AccessTokenHttpContextItemKey] = response.AccessToken;
    }

    private async Task<bool> EnsureDownstreamTokenAsync(CancellationToken cancellationToken)
    {
        var accessToken = await GetAuthenticationTokenAsync(DownstreamRequestContextHandler.AccessTokenAuthenticationTokenName);
        var accessTokenExpiresAtUtcText = await GetAuthenticationTokenAsync(AccessTokenExpiresAtUtcAuthenticationTokenName);

        if (!string.IsNullOrWhiteSpace(accessToken)
            && TryParseDateTimeOffset(accessTokenExpiresAtUtcText, out var accessTokenExpiresAtUtc)
            && accessTokenExpiresAtUtc > DateTimeOffset.UtcNow.Add(DownstreamTokenRefreshSkew))
        {
            return true;
        }

        return await RefreshSessionAsync(cancellationToken) is not null;
    }

    private async Task<AuthResponse?> RefreshSessionAsync(CancellationToken cancellationToken)
    {
        var refreshToken = await GetAuthenticationTokenAsync(RefreshTokenAuthenticationTokenName);

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return null;
        }

        try
        {
            var response = await _authApiClient.RefreshTokenAsync(new RefreshTokenRequest(refreshToken), cancellationToken);
            await SignInAsync(response);

            return response;
        }
        catch (DownstreamHttpException exception) when (exception.StatusCode == StatusCodes.Status401Unauthorized)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return null;
        }
    }

    private async Task<string?> GetAuthenticationTokenAsync(string tokenName)
    {
        var authenticationResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return authenticationResult.Succeeded
            ? authenticationResult.Properties?.GetTokenValue(tokenName)
            : null;
    }

    private static bool TryParseDateTimeOffset(string? value, out DateTimeOffset dateTimeOffset)
    {
        return DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateTimeOffset);
    }

    private static CurrentUserResponse ToCurrentUserResponse(AuthResponse response)
    {
        return new CurrentUserResponse(response.UserId, response.Email, response.UserName, response.FirstName, response.LastName);
    }
}
