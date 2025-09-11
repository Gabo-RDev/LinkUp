using LinkUp.Application.Interfaces.Repository;
using LinkUp.Application.Pagination;
using LinkUp.Domain.Models;
using LinkUp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class PostRepository(LinkUpDbContext context): GenericRepository<Post>(context), IPostRepository
{
    public async Task<PagedResult<Post>> GetPostsByCategoryAsync(Guid categoryId, int page, int size,
        CancellationToken cancellationToken)
    {
     var total = await context.Set<Post>()
         .AsNoTracking()
         .Where(n => n.CategoryId == categoryId)
         .CountAsync(cancellationToken);
     
     var posts = await context.Set<Post>()
         .AsNoTracking()
         .Where(n => n.CategoryId == categoryId)
         .OrderByDescending(n => n.CreatedAt)
         .Skip((page - 1) * size)
         .Take(size)
         .ToListAsync(cancellationToken);

     return new PagedResult<Post>(posts, total, page, size);
    }

    public async Task<PagedResult<Post>> GetPostsByAdminAsync(Guid adminId, int page, int size,
        CancellationToken cancellationToken)
    {
        var total = await context.Set<Post>()
            .AsNoTracking()
            .Where(n => n.AdminId == adminId)
            .CountAsync(cancellationToken);
        
        var posts = await context.Set<Post>()
            .AsNoTracking()
            .Where(n => n.AdminId == adminId)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);
        
        return new PagedResult<Post>(posts, total, page, size);

    }

    public async Task<PagedResult<Post>> GetRecentPostsAsync(int page, int size, Guid categoryId, CancellationToken cancellationToken)
    {
        var total = await context.Set<Post>()
            .AsNoTracking()
            .Where(n => n.CategoryId == categoryId)
            .CountAsync(cancellationToken);
        
        var posts = await context.Set<Post>()
            .AsNoTracking()
            .Where(n => n.CategoryId == categoryId)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);
        
        return new PagedResult<Post>(posts, total, page, size);
    }
}