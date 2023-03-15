using System.ComponentModel.DataAnnotations;
using CommandService.Models.Platforms;

namespace CommandService.Models.Commands;

public class Command
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int PlatformId { get; set; }

    public Platform Platform { get; set; } = null!;

    [Required]
    public string HowTo { get; set; } = null!;

    [Required]
    public string CommandLine { get; set; } = null!;
}