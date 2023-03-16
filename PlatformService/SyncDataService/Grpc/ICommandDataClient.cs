using PlatformService.Models.Platforms;

namespace PlatformService.SyncDataService.Grpc;

public interface ICommandDataClient
{
    void PublishPlatform(Platform platform);
}