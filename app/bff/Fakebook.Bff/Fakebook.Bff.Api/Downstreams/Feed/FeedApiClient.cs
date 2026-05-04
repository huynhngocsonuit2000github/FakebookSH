using Fakebook.Bff.Api.Downstreams.Cores;

namespace Fakebook.Bff.Api.Downstreams.Feed;

public sealed class FeedApiClient : HttpClientHandlerBase, IFeedApiClient
{
    public FeedApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<IReadOnlyList<FeedPostResponse>> GetFeedAsync(CancellationToken cancellationToken)
    {
        return GetForJsonAsync<IReadOnlyList<FeedPostResponse>>("api/feed", cancellationToken);
    }

    public Task<IReadOnlyList<FeedPostResponse>> GetOwnPostsAsync(CancellationToken cancellationToken)
    {
        return GetForJsonAsync<IReadOnlyList<FeedPostResponse>>("api/feed/me/posts", cancellationToken);
    }

    public Task<IReadOnlyList<FeedPostResponse>> GetSavedPostsAsync(CancellationToken cancellationToken)
    {
        return GetForJsonAsync<IReadOnlyList<FeedPostResponse>>("api/feed/saved", cancellationToken);
    }

    public Task<FeedPostResponse> CreatePostAsync(CreatePostRequest request, CancellationToken cancellationToken)
    {
        return PostForJsonAsync<CreatePostRequest, FeedPostResponse>("api/feed/posts", request, cancellationToken);
    }

    public Task<FeedPostResponse> ToggleReactionAsync(Guid postId, CancellationToken cancellationToken)
    {
        return PostForJsonAsync<object, FeedPostResponse>($"api/feed/posts/{postId}/reaction", new { }, cancellationToken);
    }

    public Task<FeedPostResponse> AddCommentAsync(Guid postId, AddCommentRequest request, CancellationToken cancellationToken)
    {
        return PostForJsonAsync<AddCommentRequest, FeedPostResponse>($"api/feed/posts/{postId}/comments", request, cancellationToken);
    }

    public Task<FeedPostResponse> ToggleSavedPostAsync(Guid postId, CancellationToken cancellationToken)
    {
        return PostForJsonAsync<object, FeedPostResponse>($"api/feed/posts/{postId}/saved", new { }, cancellationToken);
    }
}
