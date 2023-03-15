using AutoMapper;
using MediatR;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models.Platforms.Queries;

namespace PlatformService.Models.Platforms.Handlers;

public class GetAllPlatformsHandler : IRequestHandler<GetAllPlatformsQuery, IEnumerable<PlatformReadDto>>
{
    private readonly IMapper _mapper;
    private readonly IPlatformRepo _platformRepo;

    public GetAllPlatformsHandler(IPlatformRepo platformRepo, IMapper mapper)
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
    }

    public Task<IEnumerable<PlatformReadDto>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
    {
        var platforms = _platformRepo.GetAllPlatforms();

        return Task.FromResult(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }
}