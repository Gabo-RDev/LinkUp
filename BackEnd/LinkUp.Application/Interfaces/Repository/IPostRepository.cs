using LinkUp.Application.Pagination;
using LinkUp.Domain.Models;

namespace LinkUp.Application.Interfaces.Repository;

public interface IPostRepository : IGenericRepository<Post>
{
    /// <summary>
    /// Retrieves posts that belong to a specific category in a paginated way.
    /// </summary>
    /// <param name="categoryId">The ID of the category.</param>
    /// <param name="page">The page number (starting from 1).</param>
    /// <param name="size">The number of posts per page.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A paginated list of posts within the given category.</returns>
    Task<PagedResult<Post>> GetPostsByCategoryAsync(Guid categoryId, int page, int size,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves posts created by a specific admin in a paginated way.
    /// </summary>
    /// <param name="adminId">The ID of the admin.</param>
    /// <param name="page">The page number (starting from 1).</param>
    /// <param name="size">The number of posts per page.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A paginated list of posts authored by the given admin.</returns>
    Task<PagedResult<Post>> GetPostsByAdminAsync(Guid adminId, int page, int size,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the most recent posts, optionally filtered by category, in a paginated way.
    /// </summary>
    /// <param name="page">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of posts per page.</param>
    /// <param name="categoryId">Optional category ID to filter posts.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A paginated list of the most recent posts.</returns>
    Task<PagedResult<Post>> GetRecentPostsAsync(int page, int pageSize, Guid categoryId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all posts in a paginated way.
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of posts per page.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A paginated list of all posts.</returns>
    Task<PagedResult<Post>> GetPagedPostAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken);
}