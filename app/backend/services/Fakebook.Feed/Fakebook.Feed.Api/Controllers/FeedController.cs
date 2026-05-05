using System.Security.Claims;
using Fakebook.Feed.Application.Feed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.Feed.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/feed")]
public sealed class FeedController : ControllerBase
{
    private readonly IFeedService _feedService;

    public FeedController(IFeedService feedService)
    {
        _feedService = feedService;
    }

    [HttpGet]
    public Task<FeedPageResponse> GetFeed([FromQuery] string? cursor, [FromQuery] int? limit, CancellationToken cancellationToken)
    {
        return _feedService.GetFeedAsync(GetCurrentUser(), cursor, limit, cancellationToken);
    }

    [HttpGet("me/posts")]
    public Task<IReadOnlyList<FeedPostResponse>> GetOwnPosts(CancellationToken cancellationToken)
    {
        return _feedService.GetOwnPostsAsync(GetCurrentUser(), cancellationToken);
    }

    [HttpGet("saved")]
    public Task<IReadOnlyList<FeedPostResponse>> GetSavedPosts(CancellationToken cancellationToken)
    {
        return _feedService.GetSavedPostsAsync(GetCurrentUser(), cancellationToken);
    }

    [HttpPost("posts")]
    public Task<FeedPostResponse> CreatePost(CreatePostRequest request, CancellationToken cancellationToken)
    {
        return _feedService.CreatePostAsync(GetCurrentUser(), request, cancellationToken);
    }

    [HttpPost("posts/{postId:guid}/reaction")]
    public async Task<IActionResult> ToggleReaction(Guid postId, CancellationToken cancellationToken)
    {
        var post = await _feedService.ToggleReactionAsync(GetCurrentUser(), postId, cancellationToken);

        return post is null ? NotFound() : Ok(post);
    }

    [HttpPost("posts/{postId:guid}/comments")]
    public async Task<IActionResult> AddComment(Guid postId, AddCommentRequest request, CancellationToken cancellationToken)
    {
        var post = await _feedService.AddCommentAsync(GetCurrentUser(), postId, request, cancellationToken);

        return post is null ? NotFound() : Ok(post);
    }

    [HttpPost("posts/{postId:guid}/saved")]
    public async Task<IActionResult> ToggleSavedPost(Guid postId, CancellationToken cancellationToken)
    {
        var post = await _feedService.ToggleSavedPostAsync(GetCurrentUser(), postId, cancellationToken);

        return post is null ? NotFound() : Ok(post);
    }

    private FeedUser GetCurrentUser()
    {
        var userIdText = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdText, out var userId))
        {
            throw new InvalidOperationException("Authenticated user id is missing or invalid.");
        }

        var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        var userName = User.FindFirstValue("user_name") ?? email.Split('@')[0];
        var firstName = User.FindFirstValue(ClaimTypes.GivenName);
        var lastName = User.FindFirstValue(ClaimTypes.Surname);
        var displayName = string.Join(' ', new[] { firstName, lastName }.Where(value => !string.IsNullOrWhiteSpace(value)));

        if (string.IsNullOrWhiteSpace(displayName))
        {
            displayName = userName;
        }

        return new FeedUser(userId, email, userName, displayName, "/assets/design/1.avatar.webp");
    }
}
