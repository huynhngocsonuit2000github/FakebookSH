using Fakebook.Bff.Api.Downstreams.Auth;
using Fakebook.Bff.Api.Downstreams.Shared;
using Fakebook.BuildingBlocks.Api.Handlers;
using Microsoft.Extensions.Options;

namespace Fakebook.Bff.Api.Extensions
{
    public static class DownstreamApiClientExtensions
    {
        public static IServiceCollection AddDownstreamApiClientServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDownstreamAuthApi(configuration);

            // Later: Add additional downstream API clients here following the same pattern:

            return services;
        }

        private static IServiceCollection AddDownstreamAuthApi(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddDownstreamApiClientGeneric<IAuthApiClient, AuthApiClient, AuthApiOptions>(
                configuration,
                AuthApiOptions.SectionName);
        }

        private static IServiceCollection AddDownstreamApiClientGeneric<TClient, TImplementation, TOptions>(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName)
            where TClient : class
            where TImplementation : class, TClient
            where TOptions : DownstreamApiOptions
        {
            services
                .AddOptions<TOptions>()
                .Bind(configuration.GetSection(sectionName))
                .Validate(
                    options => Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _),
                    $"{typeof(TOptions).Name}.BaseUrl must be an absolute URL.")
                .Validate(
                    options => options.TimeoutSeconds > 0,
                    $"{typeof(TOptions).Name}.TimeoutSeconds must be greater than zero.")
                .ValidateOnStart();

            services
                .AddHttpClient<TClient, TImplementation>((serviceProvider, client) =>
                {
                    var options = serviceProvider
                        .GetRequiredService<IOptions<TOptions>>()
                        .Value;

                    client.BaseAddress = new Uri(options.BaseUrl);
                    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
                    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                })
                .AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
                .AddHttpMessageHandler<DownstreamRequestContextHandler>();

            return services;
        }
    }
}