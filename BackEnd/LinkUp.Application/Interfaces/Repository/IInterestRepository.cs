using LinkUp.Application.Pagination;
using LinkUp.Domain.Models;

namespace LinkUp.Application.Interfaces.Repository;

public interface IInterestRepository : IGenericRepository<Interest>
{
    Task<PagedResult<Interest>> GetInterestsAsync(int page, int pageSize, CancellationToken cancellationToken);
}