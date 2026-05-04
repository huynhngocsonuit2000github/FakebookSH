using Fakebook.Bff.Api.Downstreams.Feed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.Bff.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/bff/feed")]
public sealed class FeedBffController : BffControllerBase
{
    private readonly IFeedApiClient _feedApiClient;

    public FeedBffController(IFeedApiClient feedApiClient)
    {
        _feedApiClient = feedApiClient;
    }

    [HttpGet]
    public Task<IActionResult> GetFeed(CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(() => _feedApiClient.GetFeedAsync(cancellationToken));
    }

    [HttpGet("me/posts")]
    public Task<IActionResult> GetOwnPosts(CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(() => _feedApiClient.GetOwnPostsAsync(cancellationToken));
    }

    [HttpGet("saved")]
    public Task<IActionResult> GetSavedPosts(CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(() => _feedApiClient.GetSavedPostsAsync(cancellationToken));
    }

    [HttpPost("posts")]
    public Task<IActionResult> CreatePost(CreatePostRequest request, CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(() => _feedApiClient.CreatePostAsync(request, cancellationToken));
    }

    [HttpPost("posts/{postId:guid}/reaction")]
    public Task<IActionResult> ToggleReaction(Guid postId, CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(() => _feedApiClient.ToggleReactionAsync(postId, cancellationToken));
    }

    [HttpPost("posts/{postId:guid}/comments")]
    public Task<IActionResult> AddComment(Guid postId, AddCommentRequest request, CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(() => _feedApiClient.AddCommentAsync(postId, request, cancellationToken));
    }

    [HttpPost("posts/{postId:guid}/saved")]
    public Task<IActionResult> ToggleSavedPost(Guid postId, CancellationToken cancellationToken)
    {
        return ExecuteDownstreamAsync(() => _feedApiClient.ToggleSavedPostAsync(postId, cancellationToken));
    }
}
