using AutoMapper;
using MediatR;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Queries;

namespace PlatformService.Handlers;

public class GetPlatformByIdHandler : IRequestHandler<GetPlatformByIdQuery, PlatformReadDto?>
{
    private readonly IMapper _mapper;
    private readonly IPlatformRepo _platformRepo;

    public GetPlatformByIdHandler(IPlatformRepo platformRepo, IMapper mapper)
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
    }

    public Task<PlatformReadDto?> Handle(GetPlatformByIdQuery request, CancellationToken cancellationToken)
    {
        var platform = _platformRepo.GetPlatformById(request.PlatformId);
        var result = platform != null ? _mapper.Map<PlatformReadDto>(platform) : null;

        return Task.FromResult(result);
    }
}