using Fakebook.Feed.Api.Security;
using Fakebook.Feed.Application.Boundary.Repositories;
using Fakebook.Feed.Application.Feed;
using Fakebook.Feed.Infrastructure.Consumers;
using Fakebook.Feed.Infrastructure.Persistence;
using Fakebook.Feed.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Fakebook.Feed.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddFeedInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FeedDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("FeedDb"));
        });

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IPostReactionRepository, PostReactionRepository>();
        services.AddScoped<ISavedPostRepository, SavedPostRepository>();
        services.AddScoped<IPostCommentRepository, PostCommentRepository>();
        services.AddScoped<IFeedService, FeedService>();

        services.AddMassTransit(config =>
        {
            // With each consumer, MassTransit will automatically create a corresponding receive endpoint and bind it to the appropriate exchange based on the message type.
            // This allows for seamless integration with RabbitMQ and ensures that messages are routed correctly to the consumers.
            // Basically, the RabbitMQ will create a queue for each consumer and bind it to the exchange that corresponds to the message type that the consumer is consuming.
            config.AddConsumer<UserRegisteredIntegrationEventConsumer>();

            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["MessageBroker:Host"], "/", host =>
                {
                    host.Username(configuration["MessageBroker:Username"]!);
                    host.Password(configuration["MessageBroker:Password"]!);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

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
                IssuerSigningKey = RsaSecurityKeyFactory.CreatePublicKey(jwtOptions.PublicKeyPem),
                ClockSkew = TimeSpan.FromSeconds(30)
            };
        });

        services.AddAuthorization();

        return services;
    }
}