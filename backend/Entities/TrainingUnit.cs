namespace WorkPlanner.Entities
{
    public class TrainingUnit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }

        public virtual TrainingPlan TrainingPlan { get; set; }
        public int TrainingPlanId { get; set; }

        public virtual List<Exercise> ExerciseList { get; set; }

    }
}
