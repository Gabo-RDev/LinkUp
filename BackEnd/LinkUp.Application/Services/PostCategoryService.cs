using LinkUp.Application.DTos.PostsCategories;
using LinkUp.Application.Helpers;
using LinkUp.Application.Interfaces.Repository;
using LinkUp.Application.Interfaces.Services;
using LinkUp.Application.Mappers;
using LinkUp.Application.Pagination;
using LinkUp.Application.Utils;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace LinkUp.Application.Services;

public class PostCategoryService(
    ILogger<PostCategoryService> logger,
    IPostCategoryRepository postCategoryRepository,
    IDistributedCache cache
) : IPostCategoryService
{
    public async Task<ResultT<CategoryDto>> CreateAsync(CreatePostCategoryDto entity,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(entity.CategoryName))
        {
            logger.LogWarning("CategoryName is empty.");

            return ResultT<CategoryDto>.Failure(Error.Failure("400", "CategoryName is empty."));
        }

        if (await postCategoryRepository.ValidateAsync(r => r.Name == entity.CategoryName, cancellationToken))
        {
            logger.LogWarning("Category with name {CategoryName} already exists.", entity.CategoryName);

            return ResultT<CategoryDto>.Failure(Error.Failure("400", "Category with name already exists."));
        }

        var category = PostCategoryMapper.ToEntity(entity);

        await postCategoryRepository.CreateAsync(category, cancellationToken);

        logger.LogInformation("Category created successfully.");

        return ResultT<CategoryDto>.Success(PostCategoryMapper.ToDto(category));
    }

    public async Task<Result> UpdateAsync(Guid id, UpdatePostCategoryDto entity, CancellationToken cancellationToken)
    {
        var postCategory = await EntityHelper.GetEntityByIdAsync(
            postCategoryEntity => postCategoryRepository.GetByIdAsync(postCategoryEntity, cancellationToken),
            id,
            "PostCategory",
            logger
        );

        if (!postCategory.IsSuccess) return ResultT<CategoryDto>.Failure(postCategory.Error!);

        postCategory.Value.Name = entity.CategoryName;
        postCategory.Value.UpdatedAt = DateTime.UtcNow;

        await postCategoryRepository.UpdateAsync(postCategory.Value, cancellationToken);

        logger.LogInformation("PostCategory updated successfully.");

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var postCategory = await EntityHelper.GetEntityByIdAsync(
            postCategoryEntity => postCategoryRepository.GetByIdAsync(postCategoryEntity, cancellationToken),
            id,
            "PostCategory",
            logger
        );

        if (!postCategory.IsSuccess) return Result.Failure(postCategory.Error!);

        await postCategoryRepository.DeleteAsync(postCategory.Value, cancellationToken);

        logger.LogInformation("PostCategory deleted successfully.");

        return Result.Success();
    }

    public async Task<ResultT<CategoryDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var postCategory = await EntityHelper.GetEntityByIdAsync(
            postCategoryEntity => postCategoryRepository.GetByIdAsync(postCategoryEntity, cancellationToken),
            id,
            "PostCategory",
            logger
        );

        if (!postCategory.IsSuccess) return ResultT<CategoryDto>.Failure(postCategory.Error!);

        logger.LogInformation("PostCategory found successfully.");

        return ResultT<CategoryDto>.Success(PostCategoryMapper.ToDto(postCategory.Value));
    }

    public async Task<ResultT<PagedResult<CategoryDto>>> GetPagedAsync(int page, int size,
        CancellationToken cancellationToken)
    {
        var validationPagination = EntityHelper.ValidatePagination<CategoryDto>(page, size, logger);

        if (!validationPagination.IsSuccess)
            return ResultT<PagedResult<CategoryDto>>.Failure(validationPagination.Error!);

        string key = $"get-paged-posts-category-page-{page}-{size}";

        var result = await cache.GetOrCreateAsync(key, async () =>
        {
            var paged = await postCategoryRepository.GetPostCategoriesAsync(page, size, cancellationToken);

            var pagedResultDto = paged.Items.Select(PostCategoryMapper.ToDto).ToList();

            return new PagedResult<CategoryDto>(pagedResultDto, paged.TotalItems, page, size);
        }, cancellationToken: cancellationToken);

        if (!result.Items.Any())
        {
            logger.LogWarning("No categories found.");

            return ResultT<PagedResult<CategoryDto>>.Failure(Error.Failure("404", "No categories found."));
        }

        logger.LogInformation("Categories found successfully.");

        return ResultT<PagedResult<CategoryDto>>.Success(result);
    }
}