using Domain.Entities.Abstractions;
using EntityFramework;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstractions;

namespace Repositories.Implementations;

/// <summary>
/// A base repository implementation that provides common database operations 
/// using Entity Framework for entities with a soft delete mechanism.
/// </summary>
/// <typeparam name="TEntity">The type of the entity that the repository will handle.</typeparam>
/// <typeparam name="TId">The type of the entity's identifier.</typeparam>
public abstract class BaseEfRepository<TEntity, TId>
    : IReadRepository<TEntity, TId>, IWriteRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>, IDeletableSoftly
    where TId : struct
{
    /// <summary>
    /// The database context used for accessing the database.
    /// </summary>
    protected TaskPointsDbContext Context { get; set; }

    /// <summary>
    /// The DbSet for the entity type, used to perform CRUD operations.
    /// </summary>
    protected DbSet<TEntity> EntitySet { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEfRepository{TEntity, TId}"/> class.
    /// </summary>
    /// <param name="context">The database context to be used by the repository.</param>
    protected BaseEfRepository(TaskPointsDbContext context)
    {
        Context = context;
        EntitySet = Context.Set<TEntity>();
    }

    /// <summary>
    /// Retrieves all entities that are not marked as deleted.
    /// </summary>
    /// <param name="ct">The cancellation token to propagate notification that the operation should be canceled.</param>
    /// <returns>An <see cref="IQueryable{TEntity}"/> representing all entities in the repository that are not deleted.</returns>
    public virtual async Task<IQueryable<TEntity>> GetAllAsync(CancellationToken ct)
        => EntitySet.Where(t => !t.IsDeleted);

    /// <summary>
    /// Retrieves an entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <param name="ct">The cancellation token to propagate notification that the operation should be canceled.</param>
    /// <returns>The entity with the given identifier, or null if not found.</returns>
    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct)
        => await EntitySet.FirstOrDefaultAsync(t => t.Id.Equals(id), cancellationToken: ct);

    /// <summary>
    /// Adds a new entity to the repository and saves changes to the database.
    /// </summary>
    /// <param name="entity">The entity to add to the repository.</param>
    /// <param name="ct">The cancellation token to propagate notification that the operation should be canceled.</param>
    /// <returns>The added entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="entity"/> is null.</exception>
    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        var entityEntry = await EntitySet.AddAsync(entity, ct);

        await Context.SaveChangesAsync(ct);

        return entityEntry.Entity;
    }

    /// <summary>
    /// Updates an existing entity in the repository and saves changes to the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="ct">The cancellation token to propagate notification that the operation should be canceled.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="entity"/> is null.</exception>
    public virtual async Task UpdateAsync(TEntity entity, CancellationToken ct)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        EntitySet.Update(entity);

        await Context.SaveChangesAsync(ct);
    }
}