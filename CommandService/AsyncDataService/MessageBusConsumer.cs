using AutoMapper;
using CommandService.Data;
using CommandService.Models.Platforms;
using MassTransit;
using Shared.Dtos;

namespace CommandService.AsyncDataService;

public class MessageBusConsumer : IConsumer<PlatformPublishedDto>
{
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public MessageBusConsumer(IMapper mapper, ICommandRepo commandRepo)
    {
        _mapper = mapper;
        _commandRepo = commandRepo;
    }

    public Task Consume(ConsumeContext<PlatformPublishedDto> context)
    {
        var plat = _mapper.Map<Platform>(context.Message);

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

        return Task.CompletedTask;
    }
}