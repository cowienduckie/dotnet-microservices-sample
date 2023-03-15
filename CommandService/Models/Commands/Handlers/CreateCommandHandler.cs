using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models.Commands.Commands;
using MediatR;

namespace CommandService.Models.Commands.Handlers;

public class CreateCommandHandler : IRequestHandler<CreateCommandCommand, CommandReadDto?>
{
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public CreateCommandHandler(ICommandRepo commandRepo, IMapper mapper)
    {
        _commandRepo = commandRepo;
        _mapper = mapper;
    }

    public Task<CommandReadDto?> Handle(CreateCommandCommand request, CancellationToken cancellationToken)
    {
        if (!_commandRepo.PlatformExists(request.PlatformId))
        {
            return Task.FromResult<CommandReadDto?>(null);
        }

        var command = _mapper.Map<Command>(request);

        _commandRepo.CreateCommand(request.PlatformId, command);
        _commandRepo.SaveChanges();

        var readDto = _mapper.Map<CommandReadDto>(command);

        return Task.FromResult<CommandReadDto?>(readDto);
    }
}