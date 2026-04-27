using Fakebook.BuildingBlocks.Application.Abstractions.Cores;
using Fakebook.BuildingBlocks.Application.Messaging;
using MassTransit;

namespace Fakebook.BuildingBlocks.Infrastructure.Messaging
{
    public sealed class MessagePublisherBase : IMessagePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public MessagePublisherBase(IPublishEndpoint publishEndpoint, ICorrelationIdProvider correlationIdProvider)
        {
            _publishEndpoint = publishEndpoint;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken) where TMessage : class
        {
            var correlationId = _correlationIdProvider.GetOrCreate();

            await _publishEndpoint.Publish(message, publishContext =>
            {
                publishContext.CorrelationId = correlationId;
            }, cancellationToken);
        }
    }
}