using AutoMapper;
using CommandService;
using Grpc.Net.Client;
using PlatformService.Models.Platforms;

namespace PlatformService.SyncDataService.Grpc;

public class CommandDataClient : ICommandDataClient
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public CommandDataClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    public void PublishPlatform(Platform platform)
    {
        Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcCommand"]}");

        var channel = GrpcChannel.ForAddress(_configuration["GrpcCommand"]);
        var client = new GrpcCommand.GrpcCommandClient(channel);
        var request = new PublishPlatformRequest
        {
            Platform = _mapper.Map<CommandService.GrpcPlatformModel>(platform)
        };

        try
        {
            client.PublishPlatform(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not call GRPC Service: {ex.Message}");
        }
    }
}