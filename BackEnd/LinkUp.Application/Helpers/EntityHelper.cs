using LinkUp.Application.Pagination;
using LinkUp.Application.Utils;
using Microsoft.Extensions.Logging;

namespace LinkUp.Application.Helpers;

public static class EntityHelper
{
    public static async Task<ResultT<T>> GetEntityByIdAsync<T>(
        Func<Guid, Task<T>> getEntityByIdAsync,
        Guid id,
        string entityName,
        ILogger logger) where T : class
    {
        var entity = await getEntityByIdAsync(id);
        if (entity is not null)
            return ResultT<T>.Success(entity);

        logger.LogWarning("{EntityName} with ID {Id} was not found.", entityName, id);

        return ResultT<T>.Failure(Error.NotFound("404", $"{entityName} with ID {id} was not found."));
    }

    public static ResultT<PagedResult<T>> ValidatePagination<T>(int pageNumber, int pageSize, ILogger logger)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            logger.LogWarning("Invalid pagination parameters: PageNumber={PageNumber}, PageSize={PageSize}.",
                pageNumber, pageSize);

            return ResultT<PagedResult<T>>.Failure(
                Error.Failure("400",
                    "Invalid pagination parameters. PageNumber and PageSize must be greater than zero."));
        }

        return ResultT<PagedResult<T>>.Success(null); // Indicates that it is valid
    }
}