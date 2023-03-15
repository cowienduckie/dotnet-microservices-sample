using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using CommandService.Models.Commands;
using CommandService.Models.Commands.Commands;
using CommandService.Models.Commands.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommandsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetCommandsForPlatform(int platformId)
    {
        var query = new GetAllCommandsByPlatformIdQuery(platformId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public async  Task<ActionResult<CommandReadDto>> GetCommandForPlatform(int platformId, int commandId)
    {
        var query = new GetCommandByCommandIdAndPlatformIdQuery(platformId, commandId);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CommandReadDto>> CreateCommandForPlatform(int platformId, [FromBody] CreateCommandCommand command)
    {
        command.PlatformId = platformId;
        var result = await _mediator.Send(command);

        if (result == null)
        {
            return NotFound();
        }

        return CreatedAtAction(
            nameof(GetCommandForPlatform),
            new { platformId, commandId = result.Id },
            result);
    }
}