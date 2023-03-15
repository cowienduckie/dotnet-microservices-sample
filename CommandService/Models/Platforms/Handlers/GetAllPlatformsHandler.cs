using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models.Platforms.Queries;
using MediatR;

namespace CommandService.Models.Platforms.Handlers;

public class GetAllPlatformsHandler : IRequestHandler<GetAllPlatformsQuery, IEnumerable<PlatformReadDto>>
{
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public GetAllPlatformsHandler(ICommandRepo commandRepo, IMapper mapper)
    {
        _commandRepo = commandRepo;
        _mapper = mapper;
    }

    public Task<IEnumerable<PlatformReadDto>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
    {
        var platformItems = _commandRepo.GetAllPlatforms();

        return Task.FromResult(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }
}