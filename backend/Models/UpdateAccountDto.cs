using WorkPlanner.Enums;

namespace WorkPlanner.Models;

public sealed class UpdateAccountDto
{
    public string Email { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public DateTime? BirthDay { get; set; }
    public string? Description { get; set; }
    public MeasurementSystem MeasurementSystem { get; set; } = MeasurementSystem.Metric;
}
