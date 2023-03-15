using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataService.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMapper _mapper;
    private readonly IMessageBusClient _messageBusClient;
    private readonly IPlatformRepo _platformRepo;

    public PlatformsController(
        IPlatformRepo platformRepo,
        IMapper mapper,
        ICommandDataClient commandDataClient,
        IMessageBusClient messageBusClient
    )
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("--> Getting Platforms ...");

        var platforms = _platformRepo.GetAllPlatforms();

        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    [HttpGet("{id}", Name = "[action]")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        Console.WriteLine("--> Getting Platform ...");

        var platform = _platformRepo.GetPlatformById(id);

        if (platform == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PlatformReadDto>(platform));
    }

    [HttpPost]
    public ActionResult<PlatformReadDto> CreatePlatform([FromBody] PlatformCreateDto createDto)
    {
        Console.WriteLine("--> Creating Platform ...");

        var platform = _mapper.Map<Platform>(createDto);

        _platformRepo.CreatePlatform(platform);
        _platformRepo.SaveChanges();

        var readDto = _mapper.Map<PlatformReadDto>(platform);

        try
        {
            var publishedDto = _mapper.Map<PlatformPublishedDto>(readDto);
            publishedDto.Event = "Platform_Published";

            _messageBusClient.PublishNewPlatform(publishedDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send asynchronously: {ex}");
        }

        return CreatedAtRoute(nameof(GetPlatformById), new { id = readDto.Id }, readDto);
    }
}