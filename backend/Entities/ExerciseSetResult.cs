namespace WorkPlanner.Entities
{
    public class ExerciseSetResult
    {
        public int Id { get; set; }
        public int SetNumber { get; set; }
        public decimal? Weight { get; set; }
        public int? Repetitions { get; set; }
        public int? Duration { get; set; }
        public decimal? Rpe { get; set; }

        public int ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; } = null!;
    }
}
