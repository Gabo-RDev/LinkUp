using LinkUp.Application.Interfaces.Repository;
using LinkUp.Domain.Enum;
using LinkUp.Domain.Models;
using LinkUp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class AdminRepository(LinkUpDbContext context): GenericRepository<Admin>(context), IAdminRepository
{
    public async Task<bool> EmailInUseAsync(string email, Guid adminId, CancellationToken cancellationToken) =>
        await ValidateAsync(u => u.Email == email && u.Id != adminId, cancellationToken);

    public async Task<bool> UserNameInUseAsync(string userName, Guid adminId, CancellationToken cancellationToken) =>
        await ValidateAsync(u => u.Id == adminId && u.UserName == userName, cancellationToken);

    public async Task<Admin> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        await context.Set<Admin>()
            .AsNoTracking()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task UpdatePasswordAsync(Admin admin, string newPassword, CancellationToken cancellationToken)
    {
        admin.Password = newPassword;
        context.Set<Admin>().Update(admin);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Admin> GetAdminDetailsAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task BanUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        await context.Set<User>()
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(c => c.SetProperty(a => a.Status , UserStatus.Banned), cancellationToken);
    }

    public async Task UnbanUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        await context.Set<User>()
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(c => c.SetProperty(a => a.Status , UserStatus.Active), cancellationToken);
    }

    public async Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken) =>
        await ValidateAsync(a => a.Email == email, cancellationToken);
}