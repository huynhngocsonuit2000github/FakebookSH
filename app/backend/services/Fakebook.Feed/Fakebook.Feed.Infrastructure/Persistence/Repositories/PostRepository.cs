using Fakebook.BuildingBlocks.Infrastructure.Persistence.Repositories;
using Fakebook.Feed.Application.Boundary.Repositories;
using Fakebook.Feed.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Feed.Infrastructure.Persistence.Repositories;

public sealed class PostRepository : RepositoryBase<Post, FeedDbContext>, IPostRepository
{
    public PostRepository(FeedDbContext dbContext) : base(dbContext)
    {
    }

    public Task<List<Post>> GetFeedPostsAsync(CancellationToken cancellationToken)
    {
        return PostsWithDetails()
            .OrderByDescending(post => post.CreatedAtUtc)
            .ToListAsync(cancellationToken);
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

    public Task<bool> HasSavedPostsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return DbContext.SavedPosts.AnyAsync(savedPost => savedPost.UserId == userId, cancellationToken);
    }

    public Task<List<Post>> GetLatestPostsAsync(int count, CancellationToken cancellationToken)
    {
        return PostsWithDetails()
            .OrderByDescending(post => post.CreatedAtUtc)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> HasReactionAsync(Guid userId, Guid postId, CancellationToken cancellationToken)
    {
        return DbContext.PostReactions.AnyAsync(reaction => reaction.UserId == userId && reaction.PostId == postId, cancellationToken);
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
}
