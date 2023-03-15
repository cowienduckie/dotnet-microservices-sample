using MediatR;
using PlatformService.Dtos;

namespace PlatformService.Queries;

public class GetAllPlatformsQuery : IRequest<IEnumerable<PlatformReadDto>>
{
}