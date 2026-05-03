namespace Fakebook.Bff.Api.Security;

public sealed class RedisSessionOptions
{
    public const string SectionName = "Redis";

    public string ConnectionString { get; init; } = string.Empty;

    public string InstanceName { get; init; } = "Fakebook.Bff:";

    public int DefaultSessionMinutes { get; init; } = 60;
}
