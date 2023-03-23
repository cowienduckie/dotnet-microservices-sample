using MassTransit;
using Shared.Dtos;

namespace PlatformService.AsyncDataService;

public class MessageBusClient : IMessageBusClient
{
    private readonly IBus _messageBus;

    public MessageBusClient(IBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        Console.WriteLine("--> Publishing new platform to Message Bus");

        await _messageBus.Publish(platformPublishedDto);
    }
}