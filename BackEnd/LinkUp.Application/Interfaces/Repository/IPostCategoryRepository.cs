using LinkUp.Application.Pagination;
using LinkUp.Domain.Models;

namespace LinkUp.Application.Interfaces.Repository;

public interface IPostCategoryRepository : IGenericRepository<PostCategory>
{
    Task<PagedResult<PostCategory>> GetPostCategoriesAsync(int page, int pageSize, CancellationToken cancellationToken);
}