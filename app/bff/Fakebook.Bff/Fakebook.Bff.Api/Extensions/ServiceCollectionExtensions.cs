using Fakebook.Bff.Api.Downstreams.Shared;
using Fakebook.Bff.Api.Security;
using Fakebook.BuildingBlocks.Api.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace Fakebook.Bff.Api.Extensions;

public static class ServiceCollectionExtensions
{
    private const string FrontendCorsPolicy = "Fakebook-ui-cors";

    public static IServiceCollection AddBffServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();

        services.AddFakebookCorrelationIdProvider();
        services.AddFakebookCorrelationIdDelegatingHandler();
        services.AddTransient<DownstreamRequestContextHandler>();

        services.AddBffAuthentication(configuration);
        services.AddBffCors(configuration);

        services.AddDownstreamApiClientServices(configuration);

        return services;
    }

    private static IServiceCollection AddBffAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<RedisSessionOptions>()
            .Bind(configuration.GetSection(RedisSessionOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.ConnectionString), "Redis.ConnectionString is required.")
            .Validate(options => options.DefaultSessionMinutes > 0, "Redis.DefaultSessionMinutes must be greater than zero.")
            .ValidateOnStart();

        services.AddStackExchangeRedisCache(options =>
        {
            var redisOptions = configuration.GetSection(RedisSessionOptions.SectionName).Get<RedisSessionOptions>()
                ?? throw new InvalidOperationException("Redis configuration is missing.");

            options.Configuration = redisOptions.ConnectionString;
            options.InstanceName = redisOptions.InstanceName;
        });

        services.AddSingleton<ITicketStore, DistributedCacheTicketStore>();
        services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, CookieSessionStorePostConfigureOptions>();

        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "__Host-Fakebook.Bff";
                options.Cookie.HttpOnly = true;
                options.Cookie.Path = "/";
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = false;

                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static IApplicationBuilder UseBffCors(this IApplicationBuilder app)
    {
        return app.UseCors(FrontendCorsPolicy);
    }

    private static IServiceCollection AddBffCors(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy(FrontendCorsPolicy, policy =>
            {
                if (allowedOrigins.Length == 0)
                {
                    policy.SetIsOriginAllowed(_ => true);
                }
                else
                {
                    policy.WithOrigins(allowedOrigins);
                }

                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowCredentials();
            });
        });

        return services;
    }
}
