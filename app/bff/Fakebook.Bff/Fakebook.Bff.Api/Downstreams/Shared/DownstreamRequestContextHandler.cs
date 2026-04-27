namespace Fakebook.Bff.Api.Downstreams.Shared;

public sealed class DownstreamRequestContextHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DownstreamRequestContextHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authorization = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

        if (!string.IsNullOrWhiteSpace(authorization))
        {
            request.Headers.TryAddWithoutValidation("Authorization", authorization);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
