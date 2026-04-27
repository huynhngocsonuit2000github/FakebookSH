namespace Fakebook.BuildingBlocks.Messaging.Events
{
    public sealed record UserRegisteredIntegrationEvent(
        Guid UserId,
        string Email,
        string Username,
        DateTime OccurredAtUtc
    );
}