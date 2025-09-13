using LinkUp.Application.Interfaces.Repository;
using LinkUp.Application.Pagination;
using LinkUp.Domain.Models;
using LinkUp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class PostRepository(LinkUpDbContext context) : GenericRepository<Post>(context), IPostRepository
{
    public Task<PagedResult<Post>> GetPostsByCategoryAsync(Guid categoryId, int page, int size,
        CancellationToken cancellationToken)
    {
        var query = context.Set<Post>().AsNoTracking().Where(n => n.CategoryId == categoryId);
        
        return GetPagedAsync(query, page, size, cancellationToken);
    }

    public Task<PagedResult<Post>> GetPagedPostAsync(int page, int size, CancellationToken cancellationToken)
    {
        var query = context.Set<Post>().AsNoTracking();
        
        return GetPagedAsync(query, page, size, cancellationToken);
    }

    public Task<PagedResult<Post>> GetPostsByAdminAsync(Guid adminId, int page, int size,
        CancellationToken cancellationToken)
    {
        var query = context.Set<Post>().AsNoTracking().Where(n => n.AdminId == adminId);
        
        return GetPagedAsync(query, page, size, cancellationToken);
    }

    public Task<PagedResult<Post>> GetRecentPostsAsync(int page, int size, Guid categoryId,
        CancellationToken cancellationToken)
    {
        var query = context.Set<Post>().AsNoTracking().Where(n => n.CategoryId == categoryId);
        
        return GetPagedAsync(query, page, size, cancellationToken);
    }

    #region Private Methods

    private async Task<PagedResult<Post>> GetPagedAsync(
        IQueryable<Post> query,
        int page,
        int size,
        CancellationToken cancellationToken)
    {
        var total = await query.CountAsync(cancellationToken);

        var posts = await query
            .OrderByDescending(n => n.CreatedAt)
            .Include(n => n.Admin)
            .Include(n => n.Category)
            .Skip((page - 1) * size)
            .Take(size)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        return new PagedResult<Post>(posts, total, page, size);
    }

    #endregion
}