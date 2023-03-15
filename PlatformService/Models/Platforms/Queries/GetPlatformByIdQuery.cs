using MediatR;
using PlatformService.Dtos;

namespace PlatformService.Models.Platforms.Queries;

public class GetPlatformByIdQuery : IRequest<PlatformReadDto?>
{
    public GetPlatformByIdQuery(int platformId)
    {
        PlatformId = platformId;
    }

    public int PlatformId { get; }
}