using Domain.Entities.Abstractions;

namespace Repositories.Abstractions;

public interface IWriteRepository<TEntity, TId>
where TEntity : IEntity<TId>
where TId : struct
{
    public Task<TEntity> AddAsync(TEntity entity, CancellationToken ct);
}