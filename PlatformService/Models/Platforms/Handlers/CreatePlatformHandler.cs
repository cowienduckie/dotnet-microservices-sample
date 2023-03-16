using AutoMapper;
using MediatR;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models.Platforms.Commands;
using PlatformService.SyncDataService.Grpc;

namespace PlatformService.Models.Platforms.Handlers;

public class CreatePlatformHandler : IRequestHandler<CreatePlatformCommand, PlatformReadDto>
{
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMapper _mapper;
    private readonly IPlatformRepo _platformRepo;

    public CreatePlatformHandler(IPlatformRepo platformRepo, IMapper mapper, ICommandDataClient commandDataClient)
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
    }

    public Task<PlatformReadDto> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
    {
        var platform = _mapper.Map<Platform>(request);

        _platformRepo.CreatePlatform(platform);
        _platformRepo.SaveChanges();

        var readDto = _mapper.Map<PlatformReadDto>(platform);

        _commandDataClient.PublishPlatform(platform);

        return Task.FromResult(readDto);
    }
}