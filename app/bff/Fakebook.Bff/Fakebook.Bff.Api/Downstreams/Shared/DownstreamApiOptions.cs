namespace Fakebook.Bff.Api.Downstreams.Shared
{
    public abstract class DownstreamApiOptions
    {
        public string BaseUrl { get; init; } = string.Empty;

        public int TimeoutSeconds { get; init; } = 30;
    }
}