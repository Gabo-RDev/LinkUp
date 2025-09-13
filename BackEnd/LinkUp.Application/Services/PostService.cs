using LinkUp.Application.DTos.Posts;
using LinkUp.Application.Helpers;
using LinkUp.Application.Interfaces.Repository;
using LinkUp.Application.Interfaces.Services;
using LinkUp.Application.Mappers;
using LinkUp.Application.Pagination;
using LinkUp.Application.Utils;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Serilog;

namespace LinkUp.Application.Services;

public class PostService(
    ILogger<PostService> logger,
    IPostRepository postRepository,
    IPostCategoryRepository postCategoryRepository,
    IAdminRepository adminRepository,
    IDistributedCache cache
) : IPostService
{
    public async Task<ResultT<PostDto>> CreateAsync(CreatePostDto entity, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(entity.Title) || string.IsNullOrEmpty(entity.Content))
        {
            logger.LogWarning("Title or Content is empty.");

            return ResultT<PostDto>.Failure(Error.Failure("400", "Title or Content is empty."));
        }

        // if (await postRepository.ValidateAsync(po => po.Title == entity.Title, cancellationToken))
        // {
        //     logger.LogWarning("Post with title {Title} already exists.", entity.Title);
        //
        //     return ResultT<PostDto>.Failure(Error.Failure("400", "Post with title already exists."));
        // }

        var category = await EntityHelper.GetEntityByIdAsync(
            id => postCategoryRepository.GetByIdAsync(id, cancellationToken),
            entity.CategoryId ?? Guid.Empty,
            "PostCategory",
            logger);

        if (!category.IsSuccess) return ResultT<PostDto>.Failure(category.Error!);

        var admin = await EntityHelper.GetEntityByIdAsync(
            id => adminRepository.GetByIdAsync(id, cancellationToken),
            entity.AuthorPostId ?? Guid.Empty,
            "Admin",
            logger
        );

        if (!admin.IsSuccess) return ResultT<PostDto>.Failure(admin.Error!);

        if (entity.LikesCount >= 1)
        {
            logger.LogWarning("LikesCount must be less than or equal to 0.");

            return ResultT<PostDto>.Failure(Error.Failure("400", "LikesCount must be less than or equal to 0."));
        }

        var post = PostMapper.ToEntity(entity);

        await postRepository.CreateAsync(post, cancellationToken);

        logger.LogInformation("Post created successfully.");

        return ResultT<PostDto>.Success(PostMapper.ToDto(post, admin.Value, category.Value));
    }

    public async Task<Result> UpdateAsync(Guid id, UpdatePostDto entity, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(entity.Title) || string.IsNullOrEmpty(entity.Content))
        {
            logger.LogWarning("Title or Content is empty.");

            return ResultT<PostDto>.Failure(Error.Failure("400", "Title or Content is empty."));
        }

        var post = await EntityHelper.GetEntityByIdAsync(
            id => postRepository.GetByIdAsync(id, cancellationToken),
            id,
            "post",
            logger);

        if (!post.IsSuccess) return ResultT<PostDto>.Failure(post.Error!);

        post.Value.Title = entity.Title;
        post.Value.Content = entity.Content;

        await postRepository.UpdateAsync(post.Value, cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var post = await EntityHelper.GetEntityByIdAsync(
            id => postRepository.GetByIdAsync(id, cancellationToken),
            id,
            "post",
            logger);

        if (!post.IsSuccess) return ResultT<PostDto>.Failure(post.Error!);

        await postRepository.DeleteAsync(post.Value, cancellationToken);

        logger.LogInformation("Post deleted successfully.");

        return Result.Success();
    }

    public async Task<ResultT<PostDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var post = await EntityHelper.GetEntityByIdAsync(
            id => postRepository.GetByIdAsync(id, cancellationToken),
            id,
            "post",
            logger);

        if (!post.IsSuccess) return ResultT<PostDto>.Failure(post.Error!);

        logger.LogInformation("Post found successfully.");

        return ResultT<PostDto>.Success(PostMapper.ToDto(post.Value, post.Value.Admin!, post.Value.Category!));
    }

    public async Task<ResultT<PagedResult<PostDto>>> GetPagedAsync(int page, int size,
        CancellationToken cancellationToken)
    {
        var validationPagination = EntityHelper.ValidatePagination<PostDto>(page, size, logger);

        if (!validationPagination.IsSuccess) return ResultT<PagedResult<PostDto>>.Failure(validationPagination.Error!);

        string key = $"posts-page-{page}-{size}";

        var result = await cache.GetOrCreateAsync(key, async () =>
        {
            var paged = await postRepository.GetPagedPostAsync(page, size, cancellationToken);

            var pagedResultDto = paged.Items.Select(PostMapper.ToDtoNavigation).ToList();

            PagedResult<PostDto> pagedResult =
                new PagedResult<PostDto>(pagedResultDto, paged.TotalItems, page, size);

            return pagedResult;
        }, cancellationToken: cancellationToken);

        if (!result.Items.Any())
        {
            logger.LogWarning("No posts found.");

            return ResultT<PagedResult<PostDto>>.Failure(Error.Failure("404", "No posts found."));
        }

        logger.LogInformation("Posts found successfully.");

        return ResultT<PagedResult<PostDto>>.Success(new PagedResult<PostDto>());
    }

    public async Task<ResultT<PagedResult<PostDto>>> GetPagedByCategoryAsync(Guid categoryId, int page, int pageSize,
        CancellationToken cancellationToken)
    {
        var validationPagination = EntityHelper.ValidatePagination<PostDto>(page, pageSize, logger);

        if (!validationPagination.IsSuccess) return ResultT<PagedResult<PostDto>>.Failure(validationPagination.Error!);

        string key = $"posts-page-{page}-{pageSize}";

        var result = await cache.GetOrCreateAsync(key, async () =>
        {
            var paged = await postRepository.GetPostsByCategoryAsync(categoryId, page, pageSize, cancellationToken);

            var pagedResultDto = paged.Items.Select(PostMapper.ToDtoNavigation).ToList();

            PagedResult<PostDto> pagedResult =
                new PagedResult<PostDto>(pagedResultDto, paged.TotalItems, page, pageSize);

            return pagedResult;
        }, cancellationToken: cancellationToken);

        if (!result.Items.Any())
        {
            logger.LogWarning("No posts found for category with id {CategoryId}.", categoryId);

            return ResultT<PagedResult<PostDto>>.Failure(Error.Failure("404", "No posts found for category with id."));
        }

        logger.LogInformation("Posts found successfully.");

        return ResultT<PagedResult<PostDto>>.Success(result);
    }

    public async Task<ResultT<PagedResult<PostDto>>> GetPagedPostByAdminAsync(Guid adminId, int page, int pageSize,
        CancellationToken cancellationToken)
    {
        var admin = await EntityHelper.GetEntityByIdAsync(
            id => adminRepository.GetByIdAsync(id, cancellationToken),
            adminId,
            "admin",
            logger);

        if (!admin.IsSuccess) return ResultT<PagedResult<PostDto>>.Failure(admin.Error!);

        var validationPagination = EntityHelper.ValidatePagination<PostDto>(page, pageSize, logger);

        if (!validationPagination.IsSuccess) return ResultT<PagedResult<PostDto>>.Failure(validationPagination.Error!);

        var key = $"get-paged-by-admin-{adminId}-posts-page-{page}-{pageSize}";

        var result = await cache.GetOrCreateAsync(key, async () =>
        {
            var paged = await postRepository.GetPostsByAdminAsync(adminId, page, pageSize, cancellationToken);

            var pagedResultDto = paged.Items.Select(PostMapper.ToDtoNavigation).ToList();

            PagedResult<PostDto> pagedResult =
                new PagedResult<PostDto>(pagedResultDto, paged.TotalItems, page, pageSize);

            return pagedResult;
        }, cancellationToken: cancellationToken);

        if (!result.Items.Any())
        {
            logger.LogWarning("No posts found for admin with id {AdminId}.", adminId);

            return ResultT<PagedResult<PostDto>>.Failure(Error.Failure("404", "No posts found for admin with id."));
        }

        logger.LogInformation("Posts found successfully.");

        return ResultT<PagedResult<PostDto>>.Success(result);
    }

    public async Task<ResultT<PagedResult<PostDto>>> GetPagedPostRecentAsync(int page, int pageSize, Guid categoryId,
        CancellationToken cancellationToken)
    {
        var category = await EntityHelper.GetEntityByIdAsync(
            id => postCategoryRepository.GetByIdAsync(id, cancellationToken),
            categoryId,
            "PostCategory",
            logger);

        if (!category.IsSuccess) return ResultT<PagedResult<PostDto>>.Failure(category.Error!);

        var validationPagination = EntityHelper.ValidatePagination<PostDto>(page, pageSize, logger);

        if (!validationPagination.IsSuccess) return ResultT<PagedResult<PostDto>>.Failure(validationPagination.Error!);

        var key = $"get-paged-recent-posts-page-{page}-{pageSize}-{categoryId}";

        var result = await cache.GetOrCreateAsync(key, async () =>
        {
            var paged = await postRepository.GetRecentPostsAsync(page, pageSize, categoryId, cancellationToken);

            var pagedResultDto = paged.Items.Select(PostMapper.ToDtoNavigation).ToList();

            PagedResult<PostDto> pagedResult =
                new PagedResult<PostDto>(pagedResultDto, paged.TotalItems, page, pageSize);

            return pagedResult;
        }, cancellationToken: cancellationToken);

        if (!result.Items.Any())
        {
            logger.LogWarning("No posts found for category with id {CategoryId}.", categoryId);

            return ResultT<PagedResult<PostDto>>.Failure(Error.Failure("404", "No posts found for category with id."));
        }

        logger.LogInformation("Posts found successfully.");

        return ResultT<PagedResult<PostDto>>.Success(result);
    }
}