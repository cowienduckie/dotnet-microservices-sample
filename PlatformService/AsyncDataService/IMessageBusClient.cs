using Shared.Dtos;

namespace PlatformService.AsyncDataService;

public interface IMessageBusClient
{
    Task PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
}