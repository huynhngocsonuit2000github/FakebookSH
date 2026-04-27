using Fakebook.BuildingBlocks.Api.Middleware;
using Microsoft.AspNetCore.Http;

namespace Fakebook.BuildingBlocks.Api.Handlers;

/// <summary>
/// CorrelationIdDelegatingHandler is a delegating handler that adds the correlation ID from the incoming HTTP context to the outgoing HTTP request headers.
/// </summary>
public sealed class CorrelationIdDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var correlationId =
            _httpContextAccessor.HttpContext?.TraceIdentifier
            ?? Guid.NewGuid().ToString("N");

        request.Headers.Remove(CorrelationIdMiddleware.HeaderName);
        request.Headers.Add(CorrelationIdMiddleware.HeaderName, correlationId);

        return base.SendAsync(request, cancellationToken);
    }
}
