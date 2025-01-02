using Domain.Entities.Abstractions;
using EntityFramework;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstractions;

namespace Repositories.Implementations;

public abstract class BaseEfRepository<TEntity, TId>
    : IReadRepository<TEntity, TId>, IWriteRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>, IDeletableSoftly
    where TId : struct
{
    protected TaskPointsDbContext Context { get; set; }

    protected DbSet<TEntity> EntitySet { get; set; }

    protected BaseEfRepository(TaskPointsDbContext context)
    {
        Context = context;
        EntitySet = Context.Set<TEntity>();
    }

    public virtual async Task<IQueryable<TEntity>> GetAllAsync(CancellationToken ct)
        => EntitySet.Where(t => !t.IsDeleted);

    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct)
        => await EntitySet.FirstOrDefaultAsync(t => t.Id.Equals(id), cancellationToken: ct);

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        var entityEntry = await EntitySet.AddAsync(entity, ct);

        await Context.SaveChangesAsync(ct);

        return entityEntry.Entity;
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken ct)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        EntitySet.Update(entity);

        await Context.SaveChangesAsync(ct);
    }
}