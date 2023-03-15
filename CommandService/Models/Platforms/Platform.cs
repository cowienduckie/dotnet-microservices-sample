using System.ComponentModel.DataAnnotations;
using CommandService.Models.Commands;

namespace CommandService.Models.Platforms;

public class Platform
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int ExternalId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public ICollection<Command> Commands { get; set; } = new List<Command>();
}