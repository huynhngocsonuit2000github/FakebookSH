using Fakebook.Auth.Domain.Entities;
using Fakebook.BuildingBlocks.Application.Abstractions.Repositories;

namespace Fakebook.Auth.Application.Boundary.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
}