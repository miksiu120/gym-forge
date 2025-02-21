using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WorkPlanner.Entities
{
    public class Exercise
    {
        public int Id { get; set; }
        public string? Description{ get; set; }
        public string Name{ get; set; }
        public int Sets { get; set; }
        public int? Repetitions { get; set; }
        public int? Duration { get; set; }
        
        public int[]? Tempo { get; set; }

        public virtual TrainingUnit? TrainingUnit { get; set; }
        public int TrainingUnitId { get; set; }
    }
}
