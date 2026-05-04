using Fakebook.Feed.Application.Boundary.Repositories;
using Fakebook.Feed.Domain.Entities;

namespace Fakebook.Feed.Application.Feed;

public sealed class FeedService : IFeedService
{
    private readonly IPostRepository _postRepository;
    private readonly IPostReactionRepository _postReactionRepository;
    private readonly ISavedPostRepository _savedPostRepository;
    private readonly IPostCommentRepository _postCommentRepository;

    public FeedService(IPostRepository postRepository, IPostReactionRepository postReactionRepository, ISavedPostRepository savedPostRepository, IPostCommentRepository postCommentRepository)
    {
        _postRepository = postRepository;
        _postReactionRepository = postReactionRepository;
        _savedPostRepository = savedPostRepository;
        _postCommentRepository = postCommentRepository;
    }

    public async Task<IReadOnlyList<FeedPostResponse>> GetFeedAsync(FeedUser currentUser, CancellationToken cancellationToken)
    {
        await EnsureCurrentUserSeedRelationsAsync(currentUser.UserId, cancellationToken);

        var posts = await _postRepository.GetFeedPostsAsync(cancellationToken);

        return posts.Select(post => ToResponse(post, currentUser.UserId)).ToList();
    }

    public async Task<IReadOnlyList<FeedPostResponse>> GetOwnPostsAsync(FeedUser currentUser, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetOwnPostsAsync(currentUser.UserId, cancellationToken);

        return posts.Select(post => ToResponse(post, currentUser.UserId)).ToList();
    }

    public async Task<IReadOnlyList<FeedPostResponse>> GetSavedPostsAsync(FeedUser currentUser, CancellationToken cancellationToken)
    {
        await EnsureCurrentUserSeedRelationsAsync(currentUser.UserId, cancellationToken);

        var posts = await _postRepository.GetSavedPostsAsync(currentUser.UserId, cancellationToken);

        return posts.Select(post => ToResponse(post, currentUser.UserId)).ToList();
    }

    public async Task<FeedPostResponse> CreatePostAsync(FeedUser currentUser, CreatePostRequest request, CancellationToken cancellationToken)
    {
        var utcNow = DateTime.UtcNow;

        var post = new Post
        {
            AuthorId = currentUser.UserId,
            Author = currentUser.DisplayName,
            Username = currentUser.UserName,
            Avatar = currentUser.Avatar,
            Visibility = NormalizeVisibility(request.Visibility),
            Content = request.Content.Trim(),
            Feeling = NormalizeOptionalText(request.Feeling),
            Location = NormalizeOptionalText(request.Location),
            Image = string.IsNullOrWhiteSpace(request.Image) ? null : request.Image.Trim(),
            BaseLikeCount = 0,
            ShareCount = 0,
            CreatedAtUtc = utcNow,
            UpdatedAtUtc = utcNow,
            Hashtags = NormalizeHashtags(request.Hashtags)
                .Select(tag => new PostHashtag { Tag = tag })
                .ToList()
        };

        await _postRepository.AddAsync(post, cancellationToken);
        await _postRepository.SaveChangesAsync(cancellationToken);

        return ToResponse(post, currentUser.UserId);
    }

    public async Task<FeedPostResponse?> ToggleReactionAsync(FeedUser currentUser, Guid postId, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdWithDetailsAsync(postId, cancellationToken);

        if (post is null)
        {
            return null;
        }

        var reaction = post.Reactions.FirstOrDefault(existingReaction => existingReaction.UserId == currentUser.UserId);

        if (reaction is null)
        {
            reaction = new PostReaction
            {
                UserId = currentUser.UserId,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                PostId = post.Id,
            };
            await _postReactionRepository.AddAsync(reaction, cancellationToken);
        }
        else
        {
            _postReactionRepository.Remove(reaction);
        }

        await _postRepository.SaveChangesAsync(cancellationToken);

        return ToResponse(post, currentUser.UserId);
    }

    public async Task<FeedPostResponse?> AddCommentAsync(FeedUser currentUser, Guid postId, AddCommentRequest request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdWithDetailsAsync(postId, cancellationToken);

        if (post is null)
        {
            return null;
        }

        var comment = new PostComment
        {
            UserId = currentUser.UserId,
            Avatar = currentUser.Avatar,
            Name = currentUser.DisplayName,
            Username = currentUser.UserName,
            Text = request.Text.Trim(),
            Likes = 0,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            PostId = post.Id
        };

        await _postCommentRepository.AddAsync(comment, cancellationToken);

        await _postRepository.SaveChangesAsync(cancellationToken);

        return ToResponse(post, currentUser.UserId);
    }

    public async Task<FeedPostResponse?> ToggleSavedPostAsync(FeedUser currentUser, Guid postId, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdWithDetailsAsync(postId, cancellationToken);

        if (post is null)
        {
            return null;
        }

        var savedPost = post.SavedBy.FirstOrDefault(existingSavedPost => existingSavedPost.UserId == currentUser.UserId);

        if (savedPost is null)
        {
            savedPost = new SavedPost
            {
                UserId = currentUser.UserId,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                PostId = post.Id,
            };

            await _savedPostRepository.AddAsync(savedPost, cancellationToken);
        }
        else
        {
            _savedPostRepository.Remove(savedPost);
        }

        await _postRepository.SaveChangesAsync(cancellationToken);

        return ToResponse(post, currentUser.UserId);
    }

    private async Task EnsureCurrentUserSeedRelationsAsync(Guid currentUserId, CancellationToken cancellationToken)
    {
        var hasSavedPosts = await _postRepository.HasSavedPostsAsync(currentUserId, cancellationToken);

        if (hasSavedPosts)
        {
            return;
        }

        var seedPosts = await _postRepository.GetLatestPostsAsync(5, cancellationToken);

        if (seedPosts.Count < 5)
        {
            return;
        }

        var hasReaction = await _postRepository.HasReactionAsync(currentUserId, seedPosts[0].Id, cancellationToken);

        if (!hasReaction)
        {
            var reaction = new PostReaction
            {
                UserId = currentUserId,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                PostId = seedPosts[0].Id,
            };
            await _postReactionRepository.AddAsync(reaction, cancellationToken);
        }

        var savedPosts = new List<SavedPost>
        {
            new SavedPost
            {
                UserId = currentUserId,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                PostId = seedPosts[1].Id,
            },
            new SavedPost
            {
                UserId = currentUserId,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                PostId = seedPosts[4].Id,
            }
        };

        await _savedPostRepository.AddRangeAsync(savedPosts, cancellationToken);

        await _postRepository.SaveChangesAsync(cancellationToken);
    }

    private static FeedPostResponse ToResponse(Post post, Guid currentUserId)
    {
        return new FeedPostResponse(
            post.Id,
            post.AuthorId,
            post.Author,
            post.Username,
            post.Avatar,
            ToRelativeTime(post.CreatedAtUtc),
            post.Visibility,
            post.Content,
            post.Feeling,
            post.Location,
            post.Hashtags.OrderBy(hashtag => hashtag.Tag).Select(hashtag => hashtag.Tag).ToList(),
            post.Image,
            post.BaseLikeCount + post.Reactions.Count,
            post.ShareCount,
            post.Reactions.Any(reaction => reaction.UserId == currentUserId),
            post.SavedBy.Any(savedPost => savedPost.UserId == currentUserId),
            post.Comments
                .OrderByDescending(comment => comment.CreatedAtUtc)
                .Select(comment => new FeedCommentResponse(comment.Id, comment.Avatar, comment.Name, comment.Username, ToRelativeTime(comment.CreatedAtUtc), comment.Text, comment.Likes))
                .ToList());
    }

    private static string NormalizeVisibility(string visibility)
    {
        return string.Equals(visibility, "public", StringComparison.OrdinalIgnoreCase)
            ? "public"
            : "friends";
    }

    private static string? NormalizeOptionalText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static List<string> NormalizeHashtags(IReadOnlyList<string>? hashtags)
    {
        if (hashtags is null || hashtags.Count == 0)
        {
            return [];
        }

        return hashtags
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Select(tag =>
            {
                var normalized = tag.Trim().Replace(" ", string.Empty);
                return normalized.StartsWith('#') ? normalized : $"#{normalized}";
            })
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string ToRelativeTime(DateTime createdAtUtc)
    {
        var elapsed = DateTime.UtcNow - DateTime.SpecifyKind(createdAtUtc, DateTimeKind.Utc);

        if (elapsed.TotalMinutes < 1)
        {
            return "Just now";
        }

        if (elapsed.TotalHours < 1)
        {
            return $"{Math.Max(1, (int)elapsed.TotalMinutes)} min";
        }

        if (elapsed.TotalDays < 1)
        {
            return $"{Math.Max(1, (int)elapsed.TotalHours)}h";
        }

        return $"{Math.Max(1, (int)elapsed.TotalDays)}d";
    }
}
