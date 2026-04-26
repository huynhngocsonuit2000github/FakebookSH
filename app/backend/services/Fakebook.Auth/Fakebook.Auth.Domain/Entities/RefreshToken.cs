using Fakebook.BuildingBlocks.Domain.Abstractions;

namespace Fakebook.Auth.Domain.Entities;

public sealed class RefreshToken : Entity
{
    public Guid UserId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTimeOffset ExpiresAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? RevokedAtUtc { get; set; }
    public User User { get; set; } = null!;
}
