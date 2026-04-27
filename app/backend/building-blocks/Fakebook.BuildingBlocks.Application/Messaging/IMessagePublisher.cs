namespace Fakebook.BuildingBlocks.Application.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken) where TMessage : class;
    }
}