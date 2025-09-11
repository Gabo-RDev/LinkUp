using LinkUp.Application.Pagination;
using LinkUp.Domain.Models;

namespace LinkUp.Application.Interfaces.Repository;

/// <summary>
/// Defines methods to interact with post likes in the database.
/// Inherits basic CRUD operations from IGenericRepository.
/// </summary>
public interface IPostLikeRepository : IGenericRepository<PostLike>
{

    /// <summary>
    /// Counts the total number of likes for a specific post.
    /// </summary>
    /// <param name="postId">The ID of the post.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The total number of likes.</returns>
    Task<int> GetLikesCountAsync(Guid postId, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if a user has already liked a specific post.
    /// </summary>
    /// <param name="postId">The ID of the post.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>True if the user has liked the post; otherwise false.</returns>
    Task<bool> HasUserLikedPostAsync(Guid postId, Guid userId, CancellationToken cancellationToken);
}