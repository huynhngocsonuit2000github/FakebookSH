using Fakebook.BuildingBlocks.Application.Abstractions.Repositories;
using Fakebook.Feed.Application.Feed;
using Fakebook.Feed.Domain.Entities;

namespace Fakebook.Feed.Application.Boundary.Repositories;

public interface IPostRepository : IRepositoryBase<Post>
{
    Task<List<Post>> GetFeedPostsAsync(FeedCursor? cursor, int limit, CancellationToken cancellationToken);

    Task<List<Post>> GetOwnPostsAsync(Guid authorId, CancellationToken cancellationToken);

    Task<List<Post>> GetSavedPostsAsync(Guid userId, CancellationToken cancellationToken);

    Task<Post?> GetByIdWithDetailsAsync(Guid postId, CancellationToken cancellationToken);
}
