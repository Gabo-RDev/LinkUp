using LinkUp.Application.DTos.Posts;
using LinkUp.Application.Interfaces.Base;
using LinkUp.Application.Pagination;
using LinkUp.Application.Utils;

namespace LinkUp.Application.Interfaces.Services;

/// <summary>
/// Service interface for managing posts, extending the generic service functionality.
/// </summary>
public interface IPostService : IGenericService<CreatePostDto, UpdatePostDto, PostDto>
{
    /// <summary>
    /// Retrieves a paged list of posts filtered by a specific category.
    /// </summary>
    /// <param name="categoryId">The ID of the category to filter posts by.</param>
    /// <param name="page">The page number for pagination.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A result containing the paged list of posts.</returns>
    Task<ResultT<PagedResult<PostDto>>> GetPagedByCategoryAsync(Guid categoryId, int page, int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a paged list of posts created by a specific admin.
    /// </summary>
    /// <param name="adminId">The ID of the admin who created the posts.</param>
    /// <param name="page">The page number for pagination.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A result containing the paged list of posts.</returns>
    Task<ResultT<PagedResult<PostDto>>> GetPagedPostByAdminAsync(Guid adminId, int page, int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a paged list of the most recent posts, optionally filtered by category.
    /// </summary>
    /// <param name="page">The page number for pagination.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="categoryId">The ID of the category to filter posts by.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A result containing the paged list of recent posts.</returns>
    Task<ResultT<PagedResult<PostDto>>> GetPagedPostRecentAsync(int page, int pageSize, Guid categoryId,
        CancellationToken cancellationToken);
}