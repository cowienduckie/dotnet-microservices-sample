using PlatformService.Dtos;

namespace PlatformService.SynDataService.Http;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto plat);
}