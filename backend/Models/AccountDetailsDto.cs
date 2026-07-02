using WorkPlanner.Enums;

namespace WorkPlanner.Models;

public sealed class AccountDetailsDto
{
    public string Nickname { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string Email { get; set; } = string.Empty;
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? BirthDay { get; set; }
    public string? Description { get; set; }
    public MeasurementSystem MeasurementSystem { get; set; }
}
