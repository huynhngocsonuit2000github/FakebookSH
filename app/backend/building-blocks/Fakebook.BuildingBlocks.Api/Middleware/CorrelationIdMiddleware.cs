using Fakebook.BuildingBlocks.Application.Abstractions.Cores;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fakebook.BuildingBlocks.Api.Middleware;

public sealed class CorrelationIdMiddleware
{
    public const string HeaderName = "X-Correlation-ID";

    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;
    private readonly ICorrelationIdProvider _correlationIdProvider;

    public CorrelationIdMiddleware(
        RequestDelegate next,
        ILogger<CorrelationIdMiddleware> logger,
        ICorrelationIdProvider correlationIdProvider)
    {
        _next = next;
        _logger = logger;
        _correlationIdProvider = correlationIdProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = _correlationIdProvider.GetOrCreateAsString();

        context.TraceIdentifier = correlationId;
        context.Response.Headers[HeaderName] = correlationId;

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId
        }))
        {
            await _next(context);
        }
    }
}