using LinkUp.Application.Pagination;
using LinkUp.Application.Utils;

namespace LinkUp.Application.Interfaces.Base;

/// <summary>
/// Defines generic CRUD operations for a service layer.
/// </summary>
/// <typeparam name="TCreate">The DTO type used when creating a new entity.</typeparam>
/// <typeparam name="TUpdate">The DTO type used when updating an existing entity.</typeparam>
/// <typeparam name="TDto">The DTO type used for returning entity data.</typeparam>
public interface IGenericService<TCreate, TUpdate, TDto>
{
    /// <summary>
    /// Creates a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The DTO containing entity data to create.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A result containing the created entity.</returns>
    Task<ResultT<TDto>> CreateAsync(TCreate entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to update.</param>
    /// <param name="entity">The DTO containing updated entity data.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result> UpdateAsync(Guid id, TUpdate entity, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes an existing entity asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A result containing the entity if found.</returns>
    Task<ResultT<TDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a paged list of entities asynchronously.
    /// </summary>
    /// <param name="page">The page number for pagination.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A result containing the paged list of entities.</returns>
    Task<ResultT<PagedResult<TDto>>> GetPagedAsync(int page, int size, CancellationToken cancellationToken);
}