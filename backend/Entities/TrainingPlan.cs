using System.Runtime.InteropServices.JavaScript;

namespace WorkPlanner.Entities
{
    public class TrainingPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public virtual List<TrainingUnit> TrainingUnitList { get; set; }
        public User Author { get; set; }
        public int AuthorId { get; set; }
    }
}
