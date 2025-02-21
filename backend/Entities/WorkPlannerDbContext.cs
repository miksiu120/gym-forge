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
                u.Property(e => e.BodyWeight).HasPrecision(5, 2);  // Użycie BodyWeight zamiast Weight
                u.HasMany(p => p.CreatedTrainingPlans)
                    .WithOne(c => c.Author)
                    .HasForeignKey(w => w.AuthorId);  // Można zachować AuthorId jako Foreign Key

            });

            modelBuilder.Entity<TrainingPlan>(t =>
            {
                t.Property(n => n.Name).IsRequired();
                t.Property(tu => tu.TrainingUnits).IsRequired();
                t.Property(f => f.From).IsRequired();
                t.Property(t => t.To).IsRequired();
                t.HasOne(h => h.Author)
                    .WithMany(w => w.CreatedTrainingPlans)
                    .HasForeignKey(h => h.AuthorId);

                t.HasMany(unit => unit.TrainingUnitList)  // Zmieniono TrainingUnits na TrainingUnitList dla klarowności
                    .WithOne(unit => unit.TrainingPlan)
                    .HasForeignKey(key => key.TrainingPlanId);
            });

            modelBuilder.Entity<TrainingUnit>(t =>
            {
                t.Property(n => n.Name).IsRequired();
                t.Property(e => e.Exercises).IsRequired();
                t.Property(st => st.StartTime).IsRequired();
                t.HasMany(e => e.ExerciseList)  // Zmieniono Exercises na ExerciseList
                    .WithOne(t => t.TrainingUnit)
                    .HasForeignKey(k => k.TrainingUnitId);

            });

            modelBuilder.Entity<Exercise>(e =>
            {
                e.Property(n => n.Name).IsRequired();
                e.Property(n => n.Sets).IsRequired();
                e.HasOne(u => u.TrainingUnit)
                    .WithMany(ee => ee.ExerciseList)  // Zmieniono Exercises na ExerciseList
                    .HasForeignKey(k => k.TrainingUnitId);

            });

        }
        
    }
}
