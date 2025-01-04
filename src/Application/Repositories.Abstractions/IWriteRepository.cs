using Domain.Entities.Abstractions;

namespace Repositories.Abstractions;

/// <summary>
/// Represents a write repository interface for managing entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being managed.</typeparam>
/// <typeparam name="TId">The type of the unique identifier for the entity.</typeparam>
public interface IWriteRepository<TEntity, in TId>
    where TEntity : IEntity<TId>
    where TId : struct
{
    /// <summary>
    /// Asynchronously adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The added entity.</returns>
    public Task<TEntity> AddAsync(TEntity entity, CancellationToken ct);

    /// <summary>
    /// Asynchronously updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    public Task UpdateAsync(TEntity entity, CancellationToken ct);
}