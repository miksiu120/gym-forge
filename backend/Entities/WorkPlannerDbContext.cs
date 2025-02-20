using Microsoft.EntityFrameworkCore;

namespace WorkPlanner.Entities
{
    public class WorkPlannerDbContext : DbContext
    {
        public WorkPlannerDbContext(DbContextOptions<WorkPlannerDbContext> options) :base(options)
        {

        }

        public DbSet<User> Users { get; set; } 
        public DbSet<TrainingPlan> TrainingPlans { get; set; }
        public DbSet<TrainingUnit> TrainingUnits { get; set; }
        public DbSet<Exercise> TimeExercises { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=9090;Database=WorkPlanner;Username=postgres;Password=1234");
        }
    }
}
