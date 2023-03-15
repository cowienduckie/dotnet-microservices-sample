using CommandService.Dtos;
using MediatR;

namespace CommandService.Models.Commands.Queries;

public class GetAllCommandsByPlatformIdQuery : IRequest<IEnumerable<CommandReadDto>>
{
    public GetAllCommandsByPlatformIdQuery(int platformId)
    {
        PlatformId = platformId;
    }

    public int PlatformId { get; }
}