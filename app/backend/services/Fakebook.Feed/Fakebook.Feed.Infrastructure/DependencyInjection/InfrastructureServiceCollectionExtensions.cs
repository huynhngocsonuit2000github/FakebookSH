using Fakebook.Feed.Infrastructure.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fakebook.Feed.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddFeedInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
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

        return services;
    }
}