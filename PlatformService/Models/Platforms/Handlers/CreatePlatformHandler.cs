using AutoMapper;
using MediatR;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models.Platforms.Commands;

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

    public Task<PlatformReadDto> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
    {
        var platform = _mapper.Map<Platform>(request);

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

        //TODO: Send new PlatformPublishedDto to CommandService through gRPC

        return Task.FromResult(readDto);
    }
}