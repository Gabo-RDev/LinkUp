using System.Linq.Expressions;
using LinkUp.Application.Interfaces.Repository;
using LinkUp.Domain.Common;
using LinkUp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class GenericRepository<TEntity>(LinkUpDbContext context)
    : IGenericRepository<TEntity> where TEntity : SoftDeletableEntity
{
    protected readonly LinkUpDbContext Context = context;

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await Context.Set<TEntity>().FindAsync([id], cancellationToken);

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Context.Set<TEntity>().Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
        await SaveAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await Context.Set<TEntity>()
            .Where(e => e.Id == entity.Id)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(e => e.Deleted, true)
                    .SetProperty(e => e.DeletedAt, DateTime.UtcNow),
                cancellationToken);
    }

    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await SaveAsync(cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken) =>
        await context.SaveChangesAsync(cancellationToken);

    public async Task<bool> ValidateAsync(
        Expression<Func<TEntity, bool>> validation,
        CancellationToken cancellationToken) =>
        await Context.Set<TEntity>()
            .AsNoTracking()
            .AnyAsync(validation, cancellationToken);
}