using LinkUp.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace LinkUp.Application.Interfaces.Repository;

public interface IAdminRepository : IGenericRepository<Admin>
{
    /// <summary>
    /// Checks if an email is already in use by another admin.
    /// </summary>
    /// <param name="email">The email to check.</param>
    /// <param name="adminId">The ID of the current admin.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>True if the email is in use; otherwise false.</returns>
    Task<bool> EmailInUseAsync(string email, Guid adminId, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if a username is already in use by another admin.
    /// </summary>
    /// <param name="userName">The username to check.</param>
    /// <param name="adminId">The ID of the current admin.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>True if the username is in use; otherwise false.</returns>
    Task<bool> UserNameInUseAsync(string userName, Guid adminId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves an admin by their email address.
    /// </summary>
    /// <param name="email">The email of the admin.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The admin with the specified email.</returns>
    Task<Admin> GetByEmailAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an admin's password.
    /// </summary>
    /// <param name="admin">The admin whose password will be updated.</param>
    /// <param name="newPassword">The new password.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UpdatePasswordAsync(Admin admin, string newPassword, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves detailed information about an admin.
    /// </summary>
    /// <param name="id">The ID of the admin.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The admin details.</returns>
    Task<Admin> GetAdminDetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Bans a user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to ban.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task BanUserAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Unbans a user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to unban.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UnbanUserAsync(Guid userId, CancellationToken cancellationToken);
}