using Fakebook.BuildingBlocks.Infrastructure.Messaging;
using Fakebook.BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Fakebook.Feed.Infrastructure.Consumers;

public sealed class UserRegisteredIntegrationEventConsumer : MessageConsumerBase<UserRegisteredIntegrationEvent>
{
    private readonly ILogger<UserRegisteredIntegrationEventConsumer> _logger;

    public UserRegisteredIntegrationEventConsumer(ILogger<UserRegisteredIntegrationEventConsumer> logger) : base(logger)
    {
        _logger = logger;
    }

    protected override Task ConsumeMessageAsync(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation("User registered event received. UserId: {UserId}, Email: {Email}, Username: {Username}, FirstName: {FirstName}, LastName: {LastName}, OccurredAtUtc: {OccurredAtUtc}", message.UserId, message.Email, message.Username, message.FirstName, message.LastName, message.OccurredAtUtc);

        return Task.CompletedTask;
    }
}
