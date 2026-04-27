using Fakebook.Auth.Application.Boundary.Repositories;
using Fakebook.Auth.Domain.Entities;
using Fakebook.BuildingBlocks.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Auth.Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenRepository : RepositoryBase<RefreshToken, AuthDbContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AuthDbContext dbContext) : base(dbContext)
    {
    }

    public Task<RefreshToken?> GetByTokenHashWithUserAsync(
        string tokenHash,
        CancellationToken cancellationToken)
    {
        return DbSet
            .Include(token => token.User)
            .FirstOrDefaultAsync(token => token.TokenHash == tokenHash, cancellationToken);
    }
}