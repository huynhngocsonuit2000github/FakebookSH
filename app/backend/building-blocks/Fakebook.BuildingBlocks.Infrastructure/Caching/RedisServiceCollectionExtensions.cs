using Fakebook.BuildingBlocks.Application.Abstractions.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fakebook.BuildingBlocks.Infrastructure.Caching;

public static class RedisServiceCollectionExtensions
{
    public static IServiceCollection AddFakebookRedisCache(this IServiceCollection services, IConfiguration configuration, string sectionName = RedisCacheOptions.SectionName)
    {
        services
            .AddOptions<RedisCacheOptions>()
            .Bind(configuration.GetSection(sectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.ConnectionString), $"{sectionName}:ConnectionString is required.")
            .Validate(options => options.DefaultExpirationMinutes > 0, $"{sectionName}:DefaultExpirationMinutes must be greater than zero.")
            .ValidateOnStart();

        services.AddStackExchangeRedisCache(options =>
        {
            var redisOptions = configuration.GetSection(sectionName).Get<RedisCacheOptions>()
                ?? throw new InvalidOperationException($"{sectionName} configuration is missing.");

            options.Configuration = redisOptions.ConnectionString;
            options.InstanceName = redisOptions.InstanceName;
        });

        services.AddSingleton<ICacheService, DistributedCacheService>();

        return services;
    }
}
