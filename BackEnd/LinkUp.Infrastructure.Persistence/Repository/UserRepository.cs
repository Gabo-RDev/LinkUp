using LinkUp.Application.Interfaces.Repository;
using LinkUp.Application.Pagination;
using LinkUp.Domain.Models;
using LinkUp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class UserRepository(LinkUpDbContext context): GenericRepository<User>(context), IUserRepository
{
    public async Task<bool> ConfirmedAccountAsync(Guid id, CancellationToken cancellationToken) =>
        await ValidateAsync(u => u.Id == id && u.ConfirmedAccount == true, cancellationToken);
    
    public async Task<bool> UserNameInUseAsync(string userName, Guid userId, CancellationToken cancellationToken) =>
        await ValidateAsync(u => u.Id == userId && u.UserName == userName, cancellationToken);

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        await context.Set<User>()
            .AsNoTracking()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync(cancellationToken);
    
    public async Task<bool> EmailInUseAsync(string email, Guid userId, CancellationToken cancellationToken) =>
        await ValidateAsync(u => u.Email == email && u.Id != userId, cancellationToken);

    public async Task UpdatePasswordAsync(User user, string newPassword, CancellationToken cancellationToken)
    {
        user.Password = newPassword;
        context.Set<User>().Update(user);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken) =>
        await ValidateAsync(u => u.Email == email, cancellationToken);

    public async Task<bool> UserNameExistAsync(string userName, CancellationToken cancellationToken) =>
        await ValidateAsync(u => u.UserName == userName, cancellationToken);
    
    public async Task<PagedResult<User>> GetLikesByPostAsync(Guid postId, int page, int size, CancellationToken cancellationToken)
    {
        var total = await context.Set<PostLike>()
            .AsNoTracking()
            .Where(x => x.PostId == postId)
            .CountAsync(cancellationToken);

        var users = await context.Set<PostLike>()
            .AsNoTracking()
            .Select(x => x.User!) 
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);
        
        return new PagedResult<User>(users, total, page, size);
    }

    public async Task<User> GetUserDetailsAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}