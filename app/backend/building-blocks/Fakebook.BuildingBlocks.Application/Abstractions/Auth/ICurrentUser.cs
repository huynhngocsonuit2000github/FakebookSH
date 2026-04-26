namespace Fakebook.BuildingBlocks.Application.Abstractions.Auth;

public interface ICurrentUser
{
    Guid? UserId { get; }

    string? Email { get; }

    bool IsAuthenticated { get; }
}