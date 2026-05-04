namespace Fakebook.Bff.Api.Downstreams.Feed;

public sealed record FeedPostResponse(
    Guid Id,
    Guid AuthorId,
    string Author,
    string Username,
    string Avatar,
    string Time,
    string Visibility,
    string Content,
    IReadOnlyList<string> Hashtags,
    string? Image,
    int LikeCount,
    int ShareCount,
    bool IsLiked,
    bool IsSaved,
    IReadOnlyList<FeedCommentResponse> Comments);

public sealed record FeedCommentResponse(
    Guid Id,
    string Avatar,
    string Name,
    string Username,
    string Time,
    string Text,
    int Likes);

public sealed record CreatePostRequest(string Content, string Visibility = "friends", string? Image = null);

public sealed record AddCommentRequest(string Text);
