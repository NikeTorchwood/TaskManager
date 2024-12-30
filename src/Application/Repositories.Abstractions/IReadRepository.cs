using Domain.Entities.Abstractions;

namespace Repositories.Abstractions;

public interface IReadRepository<TEntity, TId>
    where TEntity : IEntity<TId>
    where TId : struct
{
    public Task<IQueryable<TEntity>> GetAllAsync(CancellationToken ct);
    public Task<TEntity> GetByIdAsync(TId id, CancellationToken ct);
}