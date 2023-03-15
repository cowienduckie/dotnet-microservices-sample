using CommandService.Dtos;
using CommandService.Models.Platforms.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/[controller]")]
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
}