using Fakebook.BuildingBlocks.Application.Abstractions.Repositories;
using Fakebook.Feed.Domain.Entities;

namespace Fakebook.Feed.Application.Boundary.Repositories;

public interface IPostRepository : IRepositoryBase<Post>
{
    Task<List<Post>> GetFeedPostsAsync(CancellationToken cancellationToken);

    Task<List<Post>> GetOwnPostsAsync(Guid authorId, CancellationToken cancellationToken);

    Task<List<Post>> GetSavedPostsAsync(Guid userId, CancellationToken cancellationToken);

    Task<Post?> GetByIdWithDetailsAsync(Guid postId, CancellationToken cancellationToken);

    Task<bool> HasSavedPostsAsync(Guid userId, CancellationToken cancellationToken);

    Task<List<Post>> GetLatestPostsAsync(int count, CancellationToken cancellationToken);

    Task<bool> HasReactionAsync(Guid userId, Guid postId, CancellationToken cancellationToken);
}
