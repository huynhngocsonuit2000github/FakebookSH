using Fakebook.BuildingBlocks.Application.Abstractions.Repositories;
using Fakebook.Feed.Domain.Entities;

namespace Fakebook.Feed.Application.Boundary.Repositories;

public interface IPostCommentRepository : IRepositoryBase<PostComment>
{
}