using Domain.Entities.Abstractions;

namespace Repositories.Abstractions;

/// <summary>
/// Represents a read-only repository interface for accessing entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the unique identifier for the entity.</typeparam>
public interface IReadRepository<TEntity, in TId>
    where TEntity : IEntity<TId>
    where TId : struct
{
    /// <summary>
    /// Asynchronously retrieves all entities from the repository.
    /// </summary>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A queryable collection of entities.</returns>
    public Task<IQueryable<TEntity>> GetAllAsync(CancellationToken ct);

    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct);
}