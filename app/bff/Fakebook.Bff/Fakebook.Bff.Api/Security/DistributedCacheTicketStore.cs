using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Fakebook.Bff.Api.Security;

public sealed class DistributedCacheTicketStore : ITicketStore
{
    private const string KeyPrefix = "session:";

    private readonly IDistributedCache _cache;
    private readonly RedisSessionOptions _options;

    public DistributedCacheTicketStore(IDistributedCache cache, IOptions<RedisSessionOptions> options)
    {
        _cache = cache;
        _options = options.Value;
    }

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var key = KeyPrefix + Guid.NewGuid().ToString("N");

        await RenewAsync(key, ticket);

        return key;
    }

    public async Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        var bytes = TicketSerializer.Default.Serialize(ticket);

        await _cache.SetAsync(key, bytes, CreateCacheEntryOptions(ticket));
    }

    public async Task<AuthenticationTicket?> RetrieveAsync(string key)
    {
        var bytes = await _cache.GetAsync(key);

        return bytes is null
            ? null
            : TicketSerializer.Default.Deserialize(bytes);
    }

    public Task RemoveAsync(string key)
    {
        return _cache.RemoveAsync(key);
    }

    private DistributedCacheEntryOptions CreateCacheEntryOptions(AuthenticationTicket ticket)
    {
        if (ticket.Properties.ExpiresUtc is { } expiresUtc)
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expiresUtc
            };
        }

        return new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.DefaultSessionMinutes)
        };
    }
}