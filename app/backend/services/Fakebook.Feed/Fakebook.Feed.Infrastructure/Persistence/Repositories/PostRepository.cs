using Fakebook.BuildingBlocks.Infrastructure.Persistence.Repositories;
using Fakebook.Feed.Application.Boundary.Repositories;
using Fakebook.Feed.Application.Feed;
using Fakebook.Feed.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Feed.Infrastructure.Persistence.Repositories;

public sealed class PostRepository : RepositoryBase<Post, FeedDbContext>, IPostRepository
{
    public PostRepository(FeedDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Post>> GetFeedPostsAsync(FeedCursor? cursor, int limit, CancellationToken cancellationToken)
    {
        var postIds = await GetFeedPostIdsAsync(cursor, limit + 1, cancellationToken);

        if (postIds.Count == 0)
        {
            return [];
        }

        var posts = await PostsWithDetails()
            .Where(post => postIds.Contains(post.Id))
            .ToListAsync(cancellationToken);

        return posts
            .OrderBy(post => postIds.IndexOf(post.Id))
            .ToList();
    }

    public Task<List<Post>> GetOwnPostsAsync(Guid authorId, CancellationToken cancellationToken)
    {
        return PostsWithDetails()
            .Where(post => post.AuthorId == authorId)
            .OrderByDescending(post => post.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Post>> GetSavedPostsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return PostsWithDetails()
            .Where(post => post.SavedBy.Any(savedPost => savedPost.UserId == userId))
            .OrderByDescending(post => post.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<Post?> GetByIdWithDetailsAsync(Guid postId, CancellationToken cancellationToken)
    {
        return PostsWithDetails()
            .FirstOrDefaultAsync(post => post.Id == postId, cancellationToken);
    }

    private IQueryable<Post> PostsWithDetails()
    {
        return DbSet
            .Include(post => post.Hashtags)
            .Include(post => post.Comments)
            .Include(post => post.Reactions)
            .Include(post => post.SavedBy)
            .AsSplitQuery();
    }

    private Task<List<Guid>> GetFeedPostIdsAsync(FeedCursor? cursor, int takeCount, CancellationToken cancellationToken)
    {
        if (cursor is null)
        {
            return DbContext.Posts
                .OrderByDescending(post => post.CreatedAtUtc)
                .ThenByDescending(post => post.Id)
                .Take(takeCount)
                .Select(post => post.Id)
                .ToListAsync(cancellationToken);
        }

        return DbContext.Posts
            .FromSqlInterpolated($"""
                SELECT id, author_id, author, username, avatar, visibility, content, feeling, location, image, base_like_count, share_count, created_at_utc, updated_at_utc
                FROM posts
                WHERE created_at_utc < {cursor.CreatedAtUtc}
                   OR (created_at_utc = {cursor.CreatedAtUtc} AND id < {cursor.PostId})
                ORDER BY created_at_utc DESC, id DESC
                LIMIT {takeCount}
                """)
            .Select(post => post.Id)
            .ToListAsync(cancellationToken);
    }
}
