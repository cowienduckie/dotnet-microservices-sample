using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models.Commands.Queries;
using MediatR;

namespace CommandService.Models.Commands.Handlers;

public class
    GetCommandByCommandIdAndPlatformIdHandler : IRequestHandler<GetCommandByCommandIdAndPlatformIdQuery, CommandReadDto
        ?>
{
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public GetCommandByCommandIdAndPlatformIdHandler(IMapper mapper, ICommandRepo commandRepo)
    {
        _mapper = mapper;
        _commandRepo = commandRepo;
    }

    public Task<CommandReadDto?> Handle(GetCommandByCommandIdAndPlatformIdQuery request,
        CancellationToken cancellationToken)
    {
        if (!_commandRepo.PlatformExists(request.PlatformId))
        {
            return Task.FromResult<CommandReadDto?>(null);
        }

        var commandItem = _commandRepo.GetCommand(request.PlatformId, request.CommandId);
        var result = commandItem != null ? _mapper.Map<CommandReadDto>(commandItem) : null;

        return Task.FromResult(result);
    }
}