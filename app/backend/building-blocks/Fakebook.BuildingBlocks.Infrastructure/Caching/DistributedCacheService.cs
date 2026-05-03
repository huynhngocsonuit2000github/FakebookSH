using System.Text.Json;
using Fakebook.BuildingBlocks.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Fakebook.BuildingBlocks.Infrastructure.Caching;

public sealed class DistributedCacheService : ICacheService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly IDistributedCache _cache;
    private readonly RedisCacheOptions _options;

    public DistributedCacheService(IDistributedCache cache, IOptions<RedisCacheOptions> options)
    {
        _cache = cache;
        _options = options.Value;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var bytes = await _cache.GetAsync(key, cancellationToken);

        return bytes is null
            ? default
            : JsonSerializer.Deserialize<T>(bytes, JsonSerializerOptions);
    }

    public Task SetAsync<T>(string key, T value, CacheEntryOptions? options = null, CancellationToken cancellationToken = default)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, JsonSerializerOptions);

        return _cache.SetAsync(key, bytes, CreateDistributedCacheEntryOptions(options), cancellationToken);
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> factory, CacheEntryOptions? options = null, CancellationToken cancellationToken = default)
    {
        var bytes = await _cache.GetAsync(key, cancellationToken);

        if (bytes is not null)
        {
            return JsonSerializer.Deserialize<T>(bytes, JsonSerializerOptions)!;
        }

        var value = await factory(cancellationToken);

        await SetAsync(key, value, options, cancellationToken);

        return value;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        return _cache.RemoveAsync(key, cancellationToken);
    }

    private DistributedCacheEntryOptions CreateDistributedCacheEntryOptions(CacheEntryOptions? options)
    {
        if (options is not null)
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = options.AbsoluteExpiration,
                AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = options.SlidingExpiration
            };
        }

        return new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.DefaultExpirationMinutes)
        };
    }
}
