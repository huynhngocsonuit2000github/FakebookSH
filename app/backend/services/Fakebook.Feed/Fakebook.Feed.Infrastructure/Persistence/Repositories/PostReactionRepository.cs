using Fakebook.BuildingBlocks.Infrastructure.Persistence.Repositories;
using Fakebook.Feed.Application.Boundary.Repositories;
using Fakebook.Feed.Domain.Entities;

namespace Fakebook.Feed.Infrastructure.Persistence.Repositories;

public sealed class PostReactionRepository : RepositoryBase<PostReaction, FeedDbContext>, IPostReactionRepository
{
    public PostReactionRepository(FeedDbContext dbContext) : base(dbContext)
    {
    }
}