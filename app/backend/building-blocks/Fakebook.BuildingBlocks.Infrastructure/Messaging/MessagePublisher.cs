using Fakebook.BuildingBlocks.Application.Messaging;
using MassTransit;

namespace Fakebook.BuildingBlocks.Infrastructure.Messaging
{
    public sealed class MessagePublisher : IMessagePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MessagePublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken) where TMessage : class
        {
            await _publishEndpoint.Publish(message, cancellationToken);
        }
    }
}