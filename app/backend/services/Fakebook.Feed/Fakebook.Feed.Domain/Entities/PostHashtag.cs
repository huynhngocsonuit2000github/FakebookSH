using Fakebook.BuildingBlocks.Domain.Abstractions;

namespace Fakebook.Feed.Domain.Entities;

public sealed class PostHashtag : Entity
{
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
    public string Tag { get; set; } = string.Empty;
}
