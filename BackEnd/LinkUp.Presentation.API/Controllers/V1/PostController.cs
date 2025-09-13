using Asp.Versioning;
using LinkUp.Application.DTos.Posts;
using LinkUp.Application.Interfaces.Services;
using LinkUp.Application.Pagination;
using LinkUp.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LinkUp.Presentation.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/posts")]
[Produces("application/json")]
public class PostController(IPostService postService) : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    [SwaggerOperation(Summary = "Create a new post", Description = "Creates a new post with the given data.")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePostDto createPostDto, CancellationToken cancellationToken)
    {
        var result = await postService.CreateAsync(createPostDto, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Value.PostId }, result.Value);
    }

    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [SwaggerOperation(Summary = "Update a post", Description = "Updates an existing post by its ID.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostDto updatePostDto,
        CancellationToken cancellationToken)
    {
        var result = await postService.UpdateAsync(id, updatePostDto, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete a post", Description = "Deletes a post by its ID.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await postService.DeleteAsync(id, cancellationToken);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        return NoContent();
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get post by ID", Description = "Retrieves a post by its unique ID.")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await postService.GetByIdAsync(id, cancellationToken);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("paged")]
    [SwaggerOperation(Summary = "Get paged posts", Description = "Retrieves a paginated list of posts.")]
    [ProducesResponseType(typeof(PagedResult<PostDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await postService.GetPagedAsync(page, pageSize, cancellationToken);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("category/{categoryId:guid}")]
    [SwaggerOperation(Summary = "Get posts by category",
        Description = "Retrieves paginated posts for a specific category.")]
    [ProducesResponseType(typeof(PagedResult<PostDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPagedByCategory(Guid categoryId, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await postService.GetPagedByCategoryAsync(categoryId, page, pageSize, cancellationToken);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("admin/{adminId:guid}")]
    [SwaggerOperation(Summary = "Get posts by admin",
        Description = "Retrieves paginated posts authored by a specific admin.")]
    [ProducesResponseType(typeof(PagedResult<PostDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPagedByAdmin(Guid adminId, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await postService.GetPagedPostByAdminAsync(adminId, page, pageSize, cancellationToken);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("recent/{categoryId:guid}")]
    [SwaggerOperation(Summary = "Get recent posts by category",
        Description = "Retrieves the most recent posts for a specific category.")]
    [ProducesResponseType(typeof(PagedResult<PostDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPagedRecent(Guid categoryId, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await postService.GetPagedPostRecentAsync(page, pageSize, categoryId, cancellationToken);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        return Ok(result.Value);
    }
}