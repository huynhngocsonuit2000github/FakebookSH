namespace Fakebook.BuildingBlocks.Messaging.Events
{
    public sealed record UserRegisteredIntegrationEvent(
        Guid UserId,
        string Email,
        string Username,
        string FirstName,
        string LastName,
        DateTime OccurredAtUtc
    );
}
