using MediatR;
using PlatformService.Dtos;

namespace PlatformService.Models.Platforms.Queries;

public class GetAllPlatformsQuery : IRequest<IEnumerable<PlatformReadDto>>
{
}