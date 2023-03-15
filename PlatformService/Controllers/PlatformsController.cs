using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Dtos;
using PlatformService.Models.Platforms.Commands;
using PlatformService.Models.Platforms.Queries;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlatformsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetPlatforms()
    {
        var query = new GetAllPlatformsQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}", Name = "[action]")]
    public async Task<ActionResult<PlatformReadDto>> GetPlatformById(int id)
    {
        var query = new GetPlatformByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(
        [FromBody] CreatePlatformCommand createPlatformCommand)
    {
        var result = await _mediator.Send(createPlatformCommand);

        return CreatedAtRoute(nameof(GetPlatformById), new { id = result.Id }, result);
    }
}