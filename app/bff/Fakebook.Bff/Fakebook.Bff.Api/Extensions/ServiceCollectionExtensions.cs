using Fakebook.Bff.Api.Downstreams.Shared;
using Fakebook.BuildingBlocks.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? throw new InvalidOperationException("Jwt configuration is missing.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                    ClockSkew = TimeSpan.FromSeconds(30)
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
                    policy.AllowAnyOrigin();
                }
                else
                {
                    policy.WithOrigins(allowedOrigins);
                }

                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });
        });

        return services;
    }

    private sealed class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; init; } = string.Empty;

        public string Audience { get; init; } = string.Empty;

        public string SigningKey { get; init; } = string.Empty;
    }
}
