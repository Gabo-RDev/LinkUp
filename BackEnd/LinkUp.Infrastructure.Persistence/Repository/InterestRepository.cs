using LinkUp.Application.Interfaces.Repository;
using LinkUp.Domain.Models;
using LinkUp.Infrastructure.Persistence.Context;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class InterestRepository(LinkUpDbContext context): GenericRepository<Interest>(context), IInterestRepository
{
    
}