using AutoMapper;
using CommandService.Data;
using CommandService.Models.Platforms;
using Grpc.Core;

namespace CommandService.SyncDataServices.Grpc;

public class GrpcCommandService : GrpcCommand.GrpcCommandBase
{
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public GrpcCommandService(ICommandRepo commandRepo, IMapper mapper)
    {
        _commandRepo = commandRepo;
        _mapper = mapper;
    }

    public override Task<PublishPlatformResponse> PublishPlatform(PublishPlatformRequest request, ServerCallContext context)
    {
        var plat = _mapper.Map<Platform>(request.Platform);

        if (!_commandRepo.ExternalPlatformExists(plat.ExternalId))
        {
            _commandRepo.CreatePlatform(plat);
            _commandRepo.SaveChanges();

            Console.WriteLine("--> Platform added to DB");
        }
        else
        {
            Console.WriteLine("--> Platform already exists");
        }

        return Task.FromResult(new PublishPlatformResponse());
    }
}