using WorkPlanner.Models;

namespace WorkPlanner.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Nickname { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
        public MeasurementValue? Weight { get; set; }
        public MeasurementValue? Height { get; set; }

        public required string Role { get; set; } = "User";

        public List<TrainingPlan>? CreatedTrainingPlans { get; set; }
    }
}
