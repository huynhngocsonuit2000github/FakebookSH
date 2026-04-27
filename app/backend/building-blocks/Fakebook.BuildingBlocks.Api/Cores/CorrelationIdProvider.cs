using Fakebook.BuildingBlocks.Application.Abstractions.Cores;
using Microsoft.AspNetCore.Http;

namespace Fakebook.BuildingBlocks.Api.Cores
{
    public sealed class CorrelationIdProvider : ICorrelationIdProvider
    {
        public const string HeaderName = "X-Correlation-ID";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetOrCreate()
        {
            if (_httpContextAccessor!.HttpContext!.Request.Headers.TryGetValue(HeaderName, out var correlationId) &&
                !string.IsNullOrWhiteSpace(correlationId))
            {
                return Guid.Parse(correlationId!);
            }

            return Guid.NewGuid();
        }

        public string GetOrCreateAsString()
        {
            return GetOrCreate().ToString();
        }
    }
}