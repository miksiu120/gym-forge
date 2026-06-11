using System.ComponentModel.DataAnnotations;
using WorkPlanner.Enums;

namespace WorkPlanner.Models;

public sealed class UpdateAccountDto
{
    [Required, EmailAddress, MaxLength(160)] public string Email { get; set; } = string.Empty;
    [MaxLength(80)] public string? Name { get; set; }
    [MaxLength(80)] public string? Surname { get; set; }
    [Range(1, 500)] public double? Weight { get; set; }
    [Range(50, 300)] public double? Height { get; set; }
    public DateTime? BirthDay { get; set; }
    [MaxLength(500)] public string? Description { get; set; }
    public MeasurementSystem MeasurementSystem { get; set; } = MeasurementSystem.Metric;
}
