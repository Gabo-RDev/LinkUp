using System.Linq.Expressions;
using LinkUp.Application.Interfaces.Repository;
using LinkUp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class GenericRepository<TEntity>(LinkUpDbContext context): IGenericRepository<TEntity> where TEntity : class
{
    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await context.Set<TEntity>().FindAsync(id, cancellationToken);

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        context.Set<TEntity>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        await SaveAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        context.Remove(entity);
        await SaveAsync(cancellationToken);
    }

    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await SaveAsync(cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken) =>
        await context.SaveChangesAsync(cancellationToken);

    public async Task<bool> ValidateAsync(Expression<Func<TEntity, bool>> validation, CancellationToken cancellationToken) =>
        await context.Set<TEntity>()
            .AsNoTracking()
            .AnyAsync(validation, cancellationToken);
}