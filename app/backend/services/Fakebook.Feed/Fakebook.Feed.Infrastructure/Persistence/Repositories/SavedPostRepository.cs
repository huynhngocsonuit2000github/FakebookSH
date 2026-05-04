using Fakebook.BuildingBlocks.Infrastructure.Persistence.Repositories;
using Fakebook.Feed.Application.Boundary.Repositories;
using Fakebook.Feed.Domain.Entities;

namespace Fakebook.Feed.Infrastructure.Persistence.Repositories;

public sealed class SavedPostRepository : RepositoryBase<SavedPost, FeedDbContext>, ISavedPostRepository
{
    public SavedPostRepository(FeedDbContext dbContext) : base(dbContext)
    {
    }
}