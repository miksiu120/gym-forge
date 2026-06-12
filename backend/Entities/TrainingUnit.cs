namespace WorkPlanner.Entities
{
    public class TrainingUnit
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Notes { get; set; }

        public virtual TrainingPlan TrainingPlan { get; set; } = null!;
        public int TrainingPlanId { get; set; }

        public virtual List<Exercise> ExerciseList { get; set; } = [];

    }
}
