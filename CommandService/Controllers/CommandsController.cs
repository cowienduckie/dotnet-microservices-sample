using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICommandRepo _repository;

    public CommandsController(ICommandRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

        if (!_repository.PlatformExists(platformId))
        {
            return NotFound();
        }

        var commandItems = _repository.GetCommandsForPlatform(platformId);

        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId}");

        if (!_repository.PlatformExists(platformId))
        {
            return NotFound();
        }

        var commandItem = _repository.GetCommand(platformId, commandId);

        if (commandItem == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CommandReadDto>(commandItem));
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, [FromBody] CommandCreateDto createDto)
    {
        Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

        if (!_repository.PlatformExists(platformId))
        {
            return NotFound();
        }

        var command = _mapper.Map<Command>(createDto);

        _repository.CreateCommand(platformId, command);
        _repository.SaveChanges();

        var readDto = _mapper.Map<CommandReadDto>(command);

        return CreatedAtAction(
            nameof(GetCommandForPlatform),
            new { platformId, commandId = command.Id },
            readDto);
    }
}