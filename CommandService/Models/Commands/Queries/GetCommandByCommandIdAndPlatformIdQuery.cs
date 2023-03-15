using CommandService.Dtos;
using MediatR;

namespace CommandService.Models.Commands.Queries;

public class GetCommandByCommandIdAndPlatformIdQuery : IRequest<CommandReadDto?>
{
    public GetCommandByCommandIdAndPlatformIdQuery(int platformId, int commandId)
    {
        PlatformId = platformId;
        CommandId = commandId;
    }

    public int CommandId { get; }
    public int PlatformId { get; }
}