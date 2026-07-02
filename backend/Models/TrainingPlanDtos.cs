using WorkPlanner.Enums;

namespace WorkPlanner.Models;

public sealed class CreateTrainingPlanDto
{
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
    public List<CreateTrainingUnitDto> TrainingUnits { get; set; } = [];
}

public sealed class CreateTrainingUnitDto
{
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset StartTime { get; set; }
    public List<CreateExerciseDto> Exercises { get; set; } = [];
}

public sealed class CreateExerciseDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Sets { get; set; }
    public ExerciseType Type { get; set; }
    public int? Repetitions { get; set; }
    public int? Duration { get; set; }
    public string? Tempo { get; set; }
}

public sealed class TrainingPlanDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<TrainingUnitDto> TrainingUnits { get; set; } = [];
}

public sealed class TrainingUnitDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Notes { get; set; }
    public List<ExerciseDto> Exercises { get; set; } = [];
}

public sealed class ExerciseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Sets { get; set; }
    public ExerciseType Type { get; set; }
    public int? Repetitions { get; set; }
    public int? Duration { get; set; }
    public string? Tempo { get; set; }
    public List<ExerciseSetResultDto> SetResults { get; set; } = [];
}

public sealed class ExerciseSetResultDto
{
    public int SetNumber { get; set; }
    public decimal? Weight { get; set; }
    public int? Repetitions { get; set; }
    public int? Duration { get; set; }
    public decimal? Rpe { get; set; }
}

public sealed class DueTrainingUnitDto
{
    public int PlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public TrainingUnitDto TrainingUnit { get; set; } = new();
}

public sealed class CompleteTrainingUnitDto
{
    public string? Notes { get; set; }
    public List<CompleteExerciseDto> Exercises { get; set; } = [];
}

public sealed class CompleteExerciseDto
{
    public int ExerciseId { get; set; }
    public List<CompleteSetDto> Sets { get; set; } = [];
}

public sealed class CompleteSetDto
{
    public int SetNumber { get; set; }
    public decimal? Weight { get; set; }
    public int? Repetitions { get; set; }
    public int? Duration { get; set; }
    public decimal? Rpe { get; set; }
}

public sealed class TrainingStatisticsDto
{
    public int CompletedSessions { get; set; }
    public int CompletedSets { get; set; }
    public decimal TotalVolume { get; set; }
    public DateTime? LastCompletedAt { get; set; }
    public List<TrainingStatisticsSessionDto> RecentSessions { get; set; } = [];
    public List<TrainingWeightProgressDto> WeightProgress { get; set; } = [];
}

public sealed class TrainingStatisticsSessionDto
{
    public string PlanName { get; set; } = string.Empty;
    public string TrainingName { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
    public int ExerciseCount { get; set; }
    public int SetCount { get; set; }
}

public sealed class TrainingWeightProgressDto
{
    public string ExerciseName { get; set; } = string.Empty;
    public List<TrainingWeightPointDto> Points { get; set; } = [];
}

public sealed class TrainingWeightPointDto
{
    public DateTime CompletedAt { get; set; }
    public decimal Weight { get; set; }
}
