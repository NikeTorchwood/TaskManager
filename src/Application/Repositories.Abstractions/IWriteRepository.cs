using Domain.Entities.Abstractions;

namespace Repositories.Abstractions;

public interface IWriteRepository<TEntity, in TId>
    where TEntity : IEntity<TId>
    where TId : struct
{
    public Task<TEntity> AddAsync(TEntity entity, CancellationToken ct);
    public Task UpdateAsync(TEntity entity, CancellationToken ct);
}