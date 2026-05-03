namespace Fakebook.BuildingBlocks.Infrastructure.Caching;

public sealed class RedisCacheOptions
{
    public const string SectionName = "Redis";

    public string ConnectionString { get; init; } = string.Empty;

    public string InstanceName { get; init; } = "Fakebook:";

    public int DefaultExpirationMinutes { get; init; } = 60;
}
