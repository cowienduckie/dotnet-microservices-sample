using AutoMapper;
using CommandService.Dtos;
using CommandService.Models.Commands;
using CommandService.Models.Commands.Commands;
using CommandService.Models.Platforms;

namespace CommandService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<CreateCommandCommand, Command>();
        CreateMap<PlatformService.GrpcPlatformModel, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Commands, opt => opt.Ignore());
        CreateMap<CommandService.GrpcPlatformModel, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Commands, opt => opt.Ignore());
    }
}