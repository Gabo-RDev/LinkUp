using LinkUp.Application.Interfaces.Repository;
using LinkUp.Domain.Models;
using LinkUp.Infrastructure.Persistence.Context;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class PostCategoryRepository(LinkUpDbContext context) : GenericRepository<PostCategory>(context), IPostCategoryRepository
{
    
}