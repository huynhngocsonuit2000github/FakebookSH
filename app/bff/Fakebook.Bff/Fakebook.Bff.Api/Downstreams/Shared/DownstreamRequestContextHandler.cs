using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Fakebook.Bff.Api.Downstreams.Shared;

public sealed class DownstreamRequestContextHandler : DelegatingHandler
{
    public const string AccessTokenHttpContextItemKey = "__Fakebook.Bff.DownstreamAccessToken";
    public const string AccessTokenAuthenticationTokenName = "access_token";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public DownstreamRequestContextHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var accessToken = httpContext?.Items[AccessTokenHttpContextItemKey] as string;

        if (string.IsNullOrWhiteSpace(accessToken) && httpContext is not null)
        {
            var authenticationResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            accessToken = authenticationResult.Properties?.GetTokenValue(AccessTokenAuthenticationTokenName);
        }

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
