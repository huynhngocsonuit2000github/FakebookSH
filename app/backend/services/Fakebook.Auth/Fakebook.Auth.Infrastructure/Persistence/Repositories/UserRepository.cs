using Fakebook.Auth.Application.Boundary.Repositories;
using Fakebook.Auth.Domain.Entities;
using Fakebook.BuildingBlocks.Infrastructure.Persistence.Repositories;

namespace Fakebook.Auth.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : RepositoryBase<User, AuthDbContext>, IUserRepository
{
    public UserRepository(AuthDbContext dbContext) : base(dbContext)
    {
    }
}