using AutoMapper;
using MediatR;
using PlatformService.AsyncDataService;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models.Platforms.Commands;
using Shared.Dtos;

namespace PlatformService.Models.Platforms.Handlers;

public class CreatePlatformHandler : IRequestHandler<CreatePlatformCommand, PlatformReadDto>
{
    private readonly IMapper _mapper;
    private readonly IMessageBusClient _messageBusClient;
    private readonly IPlatformRepo _platformRepo;

    public CreatePlatformHandler(IPlatformRepo platformRepo, IMapper mapper, IMessageBusClient messageBusClient)
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
        _messageBusClient = messageBusClient;
    }

    public async Task<PlatformReadDto> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
    {
        var platform = _mapper.Map<Platform>(request);

        _platformRepo.CreatePlatform(platform);
        _platformRepo.SaveChanges();

        var readDto = _mapper.Map<PlatformReadDto>(platform);

        await _messageBusClient.PublishNewPlatform(_mapper.Map<PlatformPublishedDto>(readDto));

        return readDto;
    }
}