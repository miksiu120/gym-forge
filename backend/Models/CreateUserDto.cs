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
        
        public string? Name { get; set; }
        public string? Surname { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
