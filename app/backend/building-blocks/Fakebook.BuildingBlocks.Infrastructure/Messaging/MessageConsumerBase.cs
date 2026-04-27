using MassTransit;
using Microsoft.Extensions.Logging;

namespace Fakebook.BuildingBlocks.Infrastructure.Messaging;

public abstract class MessageConsumerBase<TMessage> : IConsumer<TMessage> where TMessage : class
{
    private readonly ILogger _logger;

    protected MessageConsumerBase(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TMessage> context)
    {
        using var scope = _logger.BeginScope(
            "MessageId={MessageId} CorrelationId={CorrelationId} MessageType={MessageType}",
            context.MessageId,
            context.CorrelationId,
            typeof(TMessage).Name);

        _logger.LogInformation("Consuming message {MessageType}", typeof(TMessage).Name);

        await ConsumeMessageAsync(context);

        _logger.LogInformation("Consumed message {MessageType}", typeof(TMessage).Name);
    }

    protected abstract Task ConsumeMessageAsync(ConsumeContext<TMessage> context);
}