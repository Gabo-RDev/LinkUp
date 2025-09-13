using LinkUp.Application.Interfaces.Repository;
using LinkUp.Application.Pagination;
using LinkUp.Domain.Models;
using LinkUp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class InterestRepository(LinkUpDbContext context) : GenericRepository<Interest>(context), IInterestRepository
{
    public async Task<PagedResult<Interest>> GetInterestsAsync(int page, int pageSize,
        CancellationToken cancellationToken)
    {
        var total = await Context.Set<Interest>().AsNoTracking().CountAsync(cancellationToken);
        
        var query = await Context.Set<Interest>()
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return new PagedResult<Interest>(query, total, page, pageSize);
    }
}