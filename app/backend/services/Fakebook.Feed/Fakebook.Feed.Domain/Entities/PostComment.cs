using Fakebook.BuildingBlocks.Domain.Abstractions;

namespace Fakebook.Feed.Domain.Entities;

public sealed class PostComment : AuditableEntity
{
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Avatar { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public int Likes { get; set; }
}
