using System.ComponentModel.DataAnnotations;
using MediatR;
using PlatformService.Dtos;

namespace PlatformService.Models.Platforms.Commands;

public class CreatePlatformCommand : IRequest<PlatformReadDto>
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Publisher { get; set; } = null!;

    [Required]
    public string Cost { get; set; } = null!;
}