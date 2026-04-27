using System.Linq.Expressions;

namespace Fakebook.BuildingBlocks.Application.Abstractions.Repositories
{
    public interface IRepositoryBase<TEntity>
    where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default);

        Task<List<TEntity>> ListAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}