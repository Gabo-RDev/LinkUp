using Asp.Versioning;
using LinkUp.Application.DTos.Interests;
using LinkUp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp.Presentation.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/interests")]
public class InterestController(IInterestService service) : ControllerBase
{
    /// <summary>
    /// Creates a new interest.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUpdateInterestDto dto,
        CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(dto, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Value.InterestId }, result.Value);
    }

    /// <summary>
    /// Updates an existing interest by id.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] CreateUpdateInterestDto dto,
        CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, dto, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Deletes an interest by id.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);

        if (!result.IsSuccess) return NotFound(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Gets an interest by id.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);

        if (!result.IsSuccess) return NotFound(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets a paged list of interests.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await service.GetPagedAsync(page, size, cancellationToken);

        if (!result.IsSuccess) return NotFound(result.Error);

        return Ok(result.Value);
    }
}