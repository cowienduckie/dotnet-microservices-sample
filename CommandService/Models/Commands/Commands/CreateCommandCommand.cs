using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CommandService.Dtos;
using MediatR;

namespace CommandService.Models.Commands.Commands;

public class CreateCommandCommand : IRequest<CommandReadDto?>
{
    [JsonIgnore]
    public int PlatformId { get; set; }
    
    [Required]
    public string HowTo { get; set; } = null!;

    [Required]
    public string CommandLine { get; set; } = null!;
}