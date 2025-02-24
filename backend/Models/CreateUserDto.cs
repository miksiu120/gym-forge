using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace WorkPlanner.Models
{
    public class CreateUserDto
    {
        [Required]
        public string Nickname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public double Height { get; set; }
        public string MeasurementValue { get; set; }
    }
}
