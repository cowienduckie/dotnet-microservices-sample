using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using CommandService.Models.Platforms;

namespace CommandService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _scopeFactory;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                break;
            default:
                Console.WriteLine("--> Could not determine the event type");
                break;
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch (eventType?.Event)
        {
            case "Platform_Published":
                Console.WriteLine("--> Platform Published Event Detected");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using var scope = _scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

        try
        {
            var plat = _mapper.Map<Platform>(platformPublishedDto);

            if (!repo.ExternalPlatformExists(plat.ExternalId))
            {
                repo.CreatePlatform(plat);
                repo.SaveChanges();

                Console.WriteLine("--> Platform added to DB");
            }
            else
            {
                Console.WriteLine("--> Platform already exists");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not add platform to DB: {ex.Message}");
        }
    }
}

internal enum EventType
{
    PlatformPublished,
    Undetermined
}