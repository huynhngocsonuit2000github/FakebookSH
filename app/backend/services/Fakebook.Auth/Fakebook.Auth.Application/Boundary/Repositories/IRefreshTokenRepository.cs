using Fakebook.Auth.Domain.Entities;
using Fakebook.BuildingBlocks.Application.Abstractions.Repositories;

namespace Fakebook.Auth.Application.Boundary.Repositories;

public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
{
    Task<RefreshToken?> GetByTokenHashWithUserAsync(string tokenHash, CancellationToken cancellationToken);
}