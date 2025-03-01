using System.ComponentModel.DataAnnotations;
using WorkPlanner.Enums;

namespace WorkPlanner.Models
{
    public class AccountDetailsDto
    {
        public string Nickname { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string Email { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public string Role { get; set; } 
    }
}
