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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<User>(u =>
            {
                u.Property(e => e.Nickname).IsRequired();
                u.Property(e => e.Email).IsRequired();
                u.Property(e => e.HashedPassword).IsRequired();
                u.Property(e => e.Role).IsRequired();
                u.Property(e => e.Weight).HasPrecision(5, 2);

                u.HasMany(p => p.CreatedTrainingPlans)
                    .WithOne(c => c.Author)
                    .HasForeignKey(w => w.AuthorId);
            });

            modelBuilder.Entity<TrainingPlan>(t =>
            {
                t.Property(n => n.Name).IsRequired();
                t.Property(tu => tu.From).IsRequired();
                t.Property(t => t.To).IsRequired();

                t.HasOne(h => h.Author)
                    .WithMany(w => w.CreatedTrainingPlans)
                    .HasForeignKey(h => h.AuthorId);

                t.HasMany(unit => unit.TrainingUnitList)
                    .WithOne(unit => unit.TrainingPlan)
                    .HasForeignKey(key => key.TrainingPlanId);
            });

            modelBuilder.Entity<TrainingUnit>(t =>
            {
                t.Property(n => n.Name).IsRequired();
                t.Property(e => e.StartTime).IsRequired();

                t.HasMany(e => e.ExerciseList)
                    .WithOne(t => t.TrainingUnit)
                    .HasForeignKey(k => k.TrainingUnitId);
            });

            modelBuilder.Entity<Exercise>(e =>
            {
                e.Property(n => n.Name).IsRequired();
                e.Property(n => n.Sets).IsRequired();

                e.HasOne(u => u.TrainingUnit)
                    .WithMany(ee => ee.ExerciseList)
                    .HasForeignKey(k => k.TrainingUnitId);
            });

        }
        
    }
}
