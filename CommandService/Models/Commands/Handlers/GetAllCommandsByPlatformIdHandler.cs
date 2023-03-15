using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models.Commands.Queries;
using MediatR;

namespace CommandService.Models.Commands.Handlers;

public class GetAllCommandsByPlatformIdHandler : IRequestHandler<GetAllCommandsByPlatformIdQuery, IEnumerable<CommandReadDto>>
{
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public GetAllCommandsByPlatformIdHandler(ICommandRepo commandRepo, IMapper mapper)
    {
        _commandRepo = commandRepo;
        _mapper = mapper;
    }

    public Task<IEnumerable<CommandReadDto>> Handle(GetAllCommandsByPlatformIdQuery request, CancellationToken cancellationToken)
    {
        if (!_commandRepo.PlatformExists(request.PlatformId))
        {
            return Task.FromResult(Enumerable.Empty<CommandReadDto>());
        }

        var commandItems = _commandRepo.GetCommandsForPlatform(request.PlatformId);

        return Task.FromResult(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
    }
}