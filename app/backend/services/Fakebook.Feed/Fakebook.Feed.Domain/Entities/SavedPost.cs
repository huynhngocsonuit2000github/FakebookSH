using Fakebook.BuildingBlocks.Domain.Abstractions;

namespace Fakebook.Feed.Domain.Entities;

public sealed class SavedPost : AuditableEntity
{
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
    public Guid UserId { get; set; }
}
