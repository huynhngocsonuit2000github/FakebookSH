namespace Fakebook.BuildingBlocks.Application.Abstractions.Caching;

public sealed class CacheEntryOptions
{
    public DateTimeOffset? AbsoluteExpiration { get; init; }

    public TimeSpan? AbsoluteExpirationRelativeToNow { get; init; }

    public TimeSpan? SlidingExpiration { get; init; }
}
