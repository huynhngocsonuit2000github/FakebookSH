namespace Fakebook.Bff.Api.Downstreams.Feed;

public interface IFeedApiClient
{
    Task<IReadOnlyList<FeedPostResponse>> GetFeedAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<FeedPostResponse>> GetOwnPostsAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<FeedPostResponse>> GetSavedPostsAsync(CancellationToken cancellationToken);

    Task<FeedPostResponse> CreatePostAsync(CreatePostRequest request, CancellationToken cancellationToken);

    Task<FeedPostResponse> ToggleReactionAsync(Guid postId, CancellationToken cancellationToken);

    Task<FeedPostResponse> AddCommentAsync(Guid postId, AddCommentRequest request, CancellationToken cancellationToken);

    Task<FeedPostResponse> ToggleSavedPostAsync(Guid postId, CancellationToken cancellationToken);
}
