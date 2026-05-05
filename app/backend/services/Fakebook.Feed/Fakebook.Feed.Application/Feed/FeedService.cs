using Fakebook.Feed.Application.Boundary.Repositories;
using Fakebook.Feed.Domain.Entities;

namespace Fakebook.Feed.Application.Feed;

public sealed class FeedService : IFeedService
{
    private const int DefaultFeedPageSize = 5;
    private const int MaxFeedPageSize = 20;

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

    public async Task<FeedPageResponse> GetFeedAsync(FeedUser currentUser, string? cursor, int? limit, CancellationToken cancellationToken)
    {
        var pageSize = NormalizePageSize(limit);
        var decodedCursor = DecodeCursor(cursor);
        var posts = await _postRepository.GetFeedPostsAsync(decodedCursor, pageSize, cancellationToken);
        var hasMore = posts.Count > pageSize;
        var pagePosts = hasMore ? posts.Take(pageSize).ToList() : posts;
        var nextCursor = hasMore ? EncodeCursor(pagePosts[^1]) : null;

        return new FeedPageResponse(
            pagePosts.Select(post => ToResponse(post, currentUser.UserId)).ToList(),
            nextCursor,
            hasMore);
    }

    public async Task<IReadOnlyList<FeedPostResponse>> GetOwnPostsAsync(FeedUser currentUser, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetOwnPostsAsync(currentUser.UserId, cancellationToken);

        return posts.Select(post => ToResponse(post, currentUser.UserId)).ToList();
    }

    public async Task<IReadOnlyList<FeedPostResponse>> GetSavedPostsAsync(FeedUser currentUser, CancellationToken cancellationToken)
    {
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

    private static int NormalizePageSize(int? requestedLimit)
    {
        if (!requestedLimit.HasValue || requestedLimit.Value <= 0)
        {
            return DefaultFeedPageSize;
        }

        return Math.Min(requestedLimit.Value, MaxFeedPageSize);
    }

    private static FeedCursor? DecodeCursor(string? cursor)
    {
        if (string.IsNullOrWhiteSpace(cursor))
        {
            return null;
        }

        try
        {
            var decoded = Convert.FromBase64String(cursor);
            var value = System.Text.Encoding.UTF8.GetString(decoded);
            var parts = value.Split('|', 2, StringSplitOptions.TrimEntries);

            if (parts.Length != 2)
            {
                throw new FormatException("Cursor must contain createdAtUtc and postId.");
            }

            var createdAtUtc = DateTime.Parse(parts[0], null, System.Globalization.DateTimeStyles.RoundtripKind);
            var postId = Guid.Parse(parts[1]);

            return new FeedCursor(createdAtUtc, postId);
        }
        catch (Exception exception) when (exception is FormatException or ArgumentException)
        {
            throw new ArgumentException("Invalid feed cursor.", nameof(cursor), exception);
        }
    }

    private static string EncodeCursor(Post post)
    {
        var cursor = $"{post.CreatedAtUtc:o}|{post.Id}";
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(cursor));
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
