using LinkUp.Application.Interfaces.Repository;
using LinkUp.Application.Pagination;
using LinkUp.Domain.Models;
using LinkUp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class PostCategoryRepository(LinkUpDbContext context)
    : GenericRepository<PostCategory>(context), IPostCategoryRepository
{
    public async Task<PagedResult<PostCategory>> GetPostCategoriesAsync(int page, int pageSize,
        CancellationToken cancellationToken)
    {
        var total = await Context.Set<PostCategory>().CountAsync(cancellationToken);

        var items = await Context.Set<PostCategory>()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<PostCategory>(items, total, page, pageSize);
    }
}