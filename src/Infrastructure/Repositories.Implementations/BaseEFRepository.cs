using Domain.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstractions;

namespace Repositories.Implementations;

public abstract class BaseEfRepository<TEntity, TId> 
    : IReadRepository<TEntity, TId>, IWriteRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>, IDeletableSoftly
    where TId : struct
{
    protected DbContext Context { get; set; }

    protected DbSet<TEntity> EntitySet { get; set; }

    protected BaseEfRepository(DbContext context)
    {
        Context = context;
        EntitySet = Context.Set<TEntity>();
    }

    public virtual async Task<IQueryable<TEntity>> GetAllAsync(CancellationToken ct)
        => EntitySet.Where(t => !t.IsDeleted);

    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct)
        => await EntitySet.FindAsync(id, ct);

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct)
    {
        var entityEntry = await EntitySet.AddAsync(entity, ct);
        
        await Context.SaveChangesAsync(ct);

        return entityEntry.Entity;
    }
}