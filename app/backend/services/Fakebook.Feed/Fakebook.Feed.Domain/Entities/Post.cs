using Fakebook.BuildingBlocks.Domain.Abstractions;

namespace Fakebook.Feed.Domain.Entities;

public sealed class Post : AuditableEntity
{
    public Guid AuthorId { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Visibility { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Image { get; set; }
    public int BaseLikeCount { get; set; }
    public int ShareCount { get; set; }
    public List<PostHashtag> Hashtags { get; set; } = [];
    public List<PostComment> Comments { get; set; } = [];
    public List<PostReaction> Reactions { get; set; } = [];
    public List<SavedPost> SavedBy { get; set; } = [];
}
