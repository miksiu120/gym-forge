using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkPlanner.Entities;
using WorkPlanner.Enums;

namespace WorkPlanner.Data;

public sealed class DatabaseSeeder
{
    public const string DemoNickname = "demo";
    public const string DemoPassword = "GymForge123!";

    private readonly WorkPlannerDbContext _database;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        WorkPlannerDbContext database,
        IPasswordHasher<User> passwordHasher,
        ILogger<DatabaseSeeder> logger)
    {
        _database = database;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _database.Users.AnyAsync(
                user => user.Nickname == DemoNickname,
                cancellationToken))
        {
            _logger.LogInformation("Demo data already exists. Skipping database seed.");
            return;
        }

        var today = DateTime.UtcNow.Date;
        var demoUser = new User
        {
            Nickname = DemoNickname,
            Name = "Alex",
            Surname = "Nowak",
            Email = "demo@gymforge.local",
            Weight = 82.4,
            Height = 181,
            CreatedAt = today.AddDays(-45),
            BirthDay = new DateTime(1995, 4, 12, 0, 0, 0, DateTimeKind.Utc),
            Description = "Demo athlete building strength with a two-week upper/lower split.",
            MeasurementSystem = MeasurementSystem.Metric,
            Role = "User"
        };
        demoUser.HashedPassword = _passwordHasher.HashPassword(demoUser, DemoPassword);

        var plan = new TrainingPlan
        {
            Name = "Two-week strength base",
            From = today.AddDays(-7),
            To = today.AddDays(7),
            Author = demoUser,
            TrainingUnitList =
            [
                CreateCompletedTraining(today.AddDays(-5).AddHours(17)),
                new TrainingUnit
                {
                    Name = "Training B · Upper body",
                    StartTime = today.AddDays(-2).AddHours(18),
                    ExerciseList =
                    [
                        RepetitiveExercise("Barbell bench press", 4, 6, [3, 1, 1, 0], "Pause briefly on the chest."),
                        RepetitiveExercise("Barbell row", 4, 8, [2, 1, 1, 1], "Keep the torso stable."),
                        TimedExercise("Farmer's carry", 3, 40, "Walk tall and keep the core braced.")
                    ]
                },
                new TrainingUnit
                {
                    Name = "Training A · Lower body",
                    StartTime = today.AddHours(17),
                    ExerciseList =
                    [
                        RepetitiveExercise("Back squat", 4, 5, [3, 1, 1, 0], "Leave one or two reps in reserve."),
                        RepetitiveExercise("Romanian deadlift", 3, 8, [3, 1, 1, 0], "Keep the bar close to the legs."),
                        TimedExercise("Plank", 3, 45, "Maintain a neutral spine.")
                    ]
                },
                new TrainingUnit
                {
                    Name = "Training B · Upper body",
                    StartTime = today.AddDays(2).AddHours(18),
                    ExerciseList =
                    [
                        RepetitiveExercise("Overhead press", 4, 6, [2, 1, 1, 0], "Finish each rep over the mid-foot."),
                        RepetitiveExercise("Pull-up", 4, 6, [2, 1, 1, 1], "Use assistance if needed."),
                        TimedExercise("Side plank", 3, 35, "Complete the duration on each side.")
                    ]
                }
            ]
        };

        demoUser.CreatedTrainingPlans = [plan];
        _database.Users.Add(demoUser);
        await _database.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Seeded demo account '{Nickname}' with a two-week training plan.",
            DemoNickname);
    }

    private static TrainingUnit CreateCompletedTraining(DateTime startTime)
    {
        var squat = RepetitiveExercise("Back squat", 3, 5, [3, 1, 1, 0], "Controlled first working session.");
        squat.SetResults =
        [
            Result(1, 90m, repetitions: 5, rpe: 7m),
            Result(2, 92.5m, repetitions: 5, rpe: 7.5m),
            Result(3, 92.5m, repetitions: 5, rpe: 8m)
        ];

        var plank = TimedExercise("Plank", 2, 40, "Breathe steadily throughout the set.");
        plank.SetResults =
        [
            Result(1, duration: 40, rpe: 7m),
            Result(2, duration: 40, rpe: 7.5m)
        ];

        return new TrainingUnit
        {
            Name = "Training A · Lower body",
            StartTime = startTime,
            CompletedAt = startTime.AddHours(1).AddMinutes(12),
            Notes = "Good session. Squats moved smoothly and the planned load felt right.",
            ExerciseList = [squat, plank]
        };
    }

    private static Exercise RepetitiveExercise(
        string name,
        int sets,
        int repetitions,
        int[] tempo,
        string description) => new()
    {
        Name = name,
        Sets = sets,
        Repetitions = repetitions,
        Tempo = tempo,
        Description = description,
        type = ExerciseType.Repetitive
    };

    private static Exercise TimedExercise(
        string name,
        int sets,
        int duration,
        string description) => new()
    {
        Name = name,
        Sets = sets,
        Duration = duration,
        Description = description,
        type = ExerciseType.Timed
    };

    private static ExerciseSetResult Result(
        int setNumber,
        decimal? weight = null,
        int? repetitions = null,
        int? duration = null,
        decimal? rpe = null) => new()
    {
        SetNumber = setNumber,
        Weight = weight,
        Repetitions = repetitions,
        Duration = duration,
        Rpe = rpe
    };
}
