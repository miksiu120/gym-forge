using System.ComponentModel.DataAnnotations;
using WorkPlanner.Enums;
using WorkPlanner.Models;

namespace WorkPlanner.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Nickname { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string HashedPassword { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Description { get; set; } 
        public MeasurementSystem MeasurementSystem{ get; set; }
         [Required]
        public string Role { get; set; } = "User";

        public List<TrainingPlan>? CreatedTrainingPlans { get; set; }
    }
}
