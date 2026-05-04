namespace Fakebook.Feed.Application.Feed;

public interface IFeedService
{
    Task<IReadOnlyList<FeedPostResponse>> GetFeedAsync(FeedUser currentUser, CancellationToken cancellationToken);

    Task<IReadOnlyList<FeedPostResponse>> GetOwnPostsAsync(FeedUser currentUser, CancellationToken cancellationToken);

    Task<IReadOnlyList<FeedPostResponse>> GetSavedPostsAsync(FeedUser currentUser, CancellationToken cancellationToken);

    Task<FeedPostResponse> CreatePostAsync(FeedUser currentUser, CreatePostRequest request, CancellationToken cancellationToken);

    Task<FeedPostResponse?> ToggleReactionAsync(FeedUser currentUser, Guid postId, CancellationToken cancellationToken);

    Task<FeedPostResponse?> AddCommentAsync(FeedUser currentUser, Guid postId, AddCommentRequest request, CancellationToken cancellationToken);

    Task<FeedPostResponse?> ToggleSavedPostAsync(FeedUser currentUser, Guid postId, CancellationToken cancellationToken);
}
