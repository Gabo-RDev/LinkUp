using LinkUp.Application.DTos.Interests;
using LinkUp.Application.Helpers;
using LinkUp.Application.Interfaces.Repository;
using LinkUp.Application.Interfaces.Services;
using LinkUp.Application.Mappers;
using LinkUp.Application.Pagination;
using LinkUp.Application.Utils;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace LinkUp.Application.Services;

public class InterestService(
    ILogger<InterestService> logger,
    IInterestRepository interestRepository,
    IDistributedCache cache
) : IInterestService
{
    public async Task<ResultT<InterestDto>> CreateAsync(CreateUpdateInterestDto entity,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(entity.Name))
        {
            logger.LogWarning("Name is empty.");

            return ResultT<InterestDto>.Failure(Error.Failure("400", "Name is empty."));
        }

        if (await interestRepository.ValidateAsync(i => i.Name == entity.Name, cancellationToken))
        {
            logger.LogWarning("Interest with name {Name} already exists.", entity.Name);

            return ResultT<InterestDto>.Failure(Error.Failure("400", "Interest with name already exists."));
        }

        var interest = InterestMapper.ToEntity(entity);

        await interestRepository.CreateAsync(interest, cancellationToken);

        logger.LogInformation("Interest '{InterestName}' created successfully with ID {InterestId}.",
            interest.Name, interest.Id);

        return ResultT<InterestDto>.Success(InterestMapper.ToDto(interest));
    }


    public async Task<Result> UpdateAsync(Guid id, CreateUpdateInterestDto entity, CancellationToken cancellationToken)
    {
        var interest = await EntityHelper.GetEntityByIdAsync(
            interestId => interestRepository.GetByIdAsync(interestId, cancellationToken),
            id,
            "Interest",
            logger
        );

        if (!interest.IsSuccess) return Result.Failure(interest.Error!);

        interest.Value.Name = entity.Name;
        interest.Value.UpdatedAt = DateTime.UtcNow;

        await interestRepository.UpdateAsync(interest.Value, cancellationToken);

        logger.LogInformation("Interest updated successfully.");

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var interest = await EntityHelper.GetEntityByIdAsync(
            interestId => interestRepository.GetByIdAsync(interestId, cancellationToken),
            id,
            "Interest",
            logger
        );

        if (!interest.IsSuccess) return Result.Failure(interest.Error!);

        await interestRepository.DeleteAsync(interest.Value, cancellationToken);

        logger.LogInformation("Interest deleted successfully.");

        return Result.Success();
    }

    public async Task<ResultT<InterestDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var interest = await EntityHelper.GetEntityByIdAsync(
            interestId => interestRepository.GetByIdAsync(interestId, cancellationToken),
            id,
            "Interest",
            logger
        );

        if (!interest.IsSuccess) return ResultT<InterestDto>.Failure(interest.Error!);

        logger.LogInformation("Interest found successfully.");

        return ResultT<InterestDto>.Success(InterestMapper.ToDto(interest.Value));
    }

    public async Task<ResultT<PagedResult<InterestDto>>> GetPagedAsync(int page, int size,
        CancellationToken cancellationToken)
    {
        var validationPagination = EntityHelper.ValidatePagination<InterestDto>(page, size, logger);

        if (!validationPagination.IsSuccess)
            return ResultT<PagedResult<InterestDto>>.Failure(validationPagination.Error!);

        string key = $"get-paged-interest-{page}-{size}";

        var result = await cache.GetOrCreateAsync(key, async () =>
        {
            var paged = await interestRepository.GetInterestsAsync(page, size, cancellationToken);

            var pagedDto = paged.Items.Select(InterestMapper.ToDto).ToList();

            return new PagedResult<InterestDto>(pagedDto, paged.TotalItems, page, size);
        }, cancellationToken: cancellationToken);

        if (!result.Items.Any())
        {
            logger.LogWarning("No interests found.");

            return ResultT<PagedResult<InterestDto>>.Failure(Error.Failure("404", "No interests found."));
        }

        logger.LogInformation("Interests found successfully.");

        return ResultT<PagedResult<InterestDto>>.Success(result);
    }
}