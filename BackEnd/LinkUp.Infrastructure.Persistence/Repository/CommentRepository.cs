using LinkUp.Application.Interfaces.Repository;
using LinkUp.Application.Pagination;
using LinkUp.Domain.Models;
using LinkUp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class CommentRepository(LinkUpDbContext context): GenericRepository<Comment>(context), ICommentRepository
{
    public async Task<PagedResult<Comment>> GetCommentsPaginatedByPostIdAsync(Guid postId, int page, int size, CancellationToken cancellationToken)
    {
        var total = await context.Set<Comment>()
            .AsNoTracking()
            .Where(x => x.PostId == postId)
            .CountAsync(cancellationToken);

        var comments = await context.Set<Comment>()
            .AsNoTracking()
            .Where(c => c.PostId == postId) 
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);
        
        return new PagedResult<Comment>(comments, total, page, size);
    }

    public async Task<int> GetCommentsCountByPostIdAsync(Guid postId, CancellationToken cancellationToken) =>
        await context.Set<Comment>()
            .Where(x => x.PostId == postId)
            .CountAsync(cancellationToken);
}