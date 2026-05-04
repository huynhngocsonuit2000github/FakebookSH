using Fakebook.BuildingBlocks.Infrastructure.Persistence.Repositories;
using Fakebook.Feed.Application.Boundary.Repositories;
using Fakebook.Feed.Domain.Entities;

namespace Fakebook.Feed.Infrastructure.Persistence.Repositories;

public sealed class PostCommentRepository : RepositoryBase<PostComment, FeedDbContext>, IPostCommentRepository
{
    public PostCommentRepository(FeedDbContext dbContext) : base(dbContext)
    {
    }
}