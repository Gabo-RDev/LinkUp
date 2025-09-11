using LinkUp.Application.Interfaces.Repository;
using LinkUp.Domain.Models;
using LinkUp.Infrastructure.Persistence.Context;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class UserInterestRepository(LinkUpDbContext context): GenericRepository<UserInterest>(context), IUserInterestRepository
{
    
}