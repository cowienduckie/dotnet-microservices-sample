using CommandService.Dtos;
using MediatR;

namespace CommandService.Models.Platforms.Queries;

public class GetAllPlatformsQuery : IRequest<IEnumerable<PlatformReadDto>>
{
}