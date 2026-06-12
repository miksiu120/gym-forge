using Microsoft.EntityFrameworkCore;
using PartyGame.Services;
using WorkPlanner.Entities;
using WorkPlanner.Enums;
using WorkPlanner.Models;

namespace WorkPlanner.Services;

public interface ITrainingPlanService
{
    Task<TrainingPlanDto> CreateAsync(CreateTrainingPlanDto request);
    Task<List<TrainingPlanDto>> GetMineAsync();
    Task<TrainingStatisticsDto> GetStatisticsAsync();
    Task<List<DueTrainingUnitDto>> GetDueAsync(DateTimeOffset through);
    Task<TrainingUnitDto> GetUnitAsync(int unitId);
    Task<TrainingUnitDto> CompleteUnitAsync(int unitId, CompleteTrainingUnitDto request);
}

public sealed class TrainingPlanService : ITrainingPlanService
{
    private readonly WorkPlannerDbContext _context;
    private readonly IHttpContextAccessorService _httpContext;

    public TrainingPlanService(WorkPlannerDbContext context, IHttpContextAccessorService httpContext)
    {
        _context = context;
        _httpContext = httpContext;
    }

    public async Task<TrainingPlanDto> CreateAsync(CreateTrainingPlanDto request)
    {
        var authorId = RequireUserId();
        var from = request.From.UtcDateTime;
        var to = request.To.UtcDateTime;

        if (from > to)
            throw new ArgumentException("The plan end date cannot be earlier than its start date.");

        if (request.TrainingUnits.Any(unit => unit.StartTime.UtcDateTime < from || unit.StartTime.UtcDateTime > to))
            throw new ArgumentException("Every training unit must be scheduled within the plan date range.");

        foreach (var exercise in request.TrainingUnits.SelectMany(unit => unit.Exercises))
        {
            if (exercise.Type == ExerciseType.Repetitive && exercise.Repetitions is null)
                throw new ArgumentException($"Repetitions are required for {exercise.Name}.");
            if (exercise.Type == ExerciseType.Timed && exercise.Duration is null)
                throw new ArgumentException($"Duration is required for {exercise.Name}.");
        }

        var plan = new TrainingPlan
        {
            Name = request.Name.Trim(),
            From = from,
            To = to,
            AuthorId = authorId,
            TrainingUnitList = request.TrainingUnits.Select(unit => new TrainingUnit
            {
                Name = unit.Name.Trim(),
                StartTime = unit.StartTime.UtcDateTime,
                ExerciseList = unit.Exercises.Select(exercise => new Exercise
                {
                    Name = exercise.Name.Trim(),
                    Description = exercise.Description?.Trim(),
                    Sets = exercise.Sets,
                    Repetitions = exercise.Type == ExerciseType.Repetitive ? exercise.Repetitions : null,
                    Duration = exercise.Type == ExerciseType.Timed ? exercise.Duration : null,
                    type = exercise.Type,
                    Tempo = string.IsNullOrWhiteSpace(exercise.Tempo)
                        ? null
                        : exercise.Tempo.Split('-').Select(int.Parse).ToArray(),
                }).ToList(),
            }).ToList(),
        };

        _context.TrainingPlans.Add(plan);
        await _context.SaveChangesAsync();
        return Map(plan);
    }

    public async Task<List<TrainingPlanDto>> GetMineAsync()
    {
        var authorId = RequireUserId();
        var plans = await _context.TrainingPlans
            .AsNoTracking()
            .Where(plan => plan.AuthorId == authorId)
            .Include(plan => plan.TrainingUnitList)
            .ThenInclude(unit => unit.ExerciseList)
            .ThenInclude(exercise => exercise.SetResults)
            .OrderByDescending(plan => plan.From)
            .ToListAsync();
        return plans.Select(Map).ToList();
    }

    public async Task<List<DueTrainingUnitDto>> GetDueAsync(DateTimeOffset through)
    {
        var authorId = RequireUserId();
        var units = await _context.TrainingUnits
            .AsNoTracking()
            .Where(unit => unit.TrainingPlan.AuthorId == authorId
                && unit.CompletedAt == null
                && unit.StartTime <= through.UtcDateTime)
            .Include(unit => unit.TrainingPlan)
            .Include(unit => unit.ExerciseList)
            .ThenInclude(exercise => exercise.SetResults)
            .OrderBy(unit => unit.StartTime)
            .ToListAsync();

        return units.Select(unit => new DueTrainingUnitDto
        {
            PlanId = unit.TrainingPlanId,
            PlanName = unit.TrainingPlan.Name,
            TrainingUnit = MapUnit(unit),
        }).ToList();
    }

    public async Task<TrainingStatisticsDto> GetStatisticsAsync()
    {
        var authorId = RequireUserId();
        var completedUnits = await _context.TrainingUnits
            .AsNoTracking()
            .Where(unit => unit.TrainingPlan.AuthorId == authorId && unit.CompletedAt != null)
            .Include(unit => unit.TrainingPlan)
            .Include(unit => unit.ExerciseList)
            .ThenInclude(exercise => exercise.SetResults)
            .OrderByDescending(unit => unit.CompletedAt)
            .ToListAsync();

        var results = completedUnits
            .SelectMany(unit => unit.ExerciseList)
            .SelectMany(exercise => exercise.SetResults);

        var weightProgress = completedUnits
            .SelectMany(unit => unit.ExerciseList.Select(exercise => new
            {
                Exercise = exercise.Name,
                CompletedAt = unit.CompletedAt!.Value,
                Weight = exercise.SetResults
                    .Where(result => result.Weight.HasValue)
                    .Select(result => result.Weight!.Value)
                    .DefaultIfEmpty()
                    .Max(),
            }))
            .Where(item => item.Weight > 0)
            .GroupBy(item => item.Exercise, StringComparer.OrdinalIgnoreCase)
            .OrderBy(group => group.Key)
            .Select(group => new TrainingWeightProgressDto
            {
                ExerciseName = group.Key,
                Points = group.OrderBy(item => item.CompletedAt).Select(item => new TrainingWeightPointDto
                {
                    CompletedAt = item.CompletedAt,
                    Weight = item.Weight,
                }).ToList(),
            }).ToList();

        return new TrainingStatisticsDto
        {
            CompletedSessions = completedUnits.Count,
            CompletedSets = results.Count(),
            TotalVolume = results
                .Where(result => result.Weight.HasValue && result.Repetitions.HasValue)
                .Sum(result => result.Weight!.Value * result.Repetitions!.Value),
            LastCompletedAt = completedUnits.FirstOrDefault()?.CompletedAt,
            WeightProgress = weightProgress,
            RecentSessions = completedUnits.Take(6).Select(unit => new TrainingStatisticsSessionDto
            {
                PlanName = unit.TrainingPlan.Name,
                TrainingName = unit.Name,
                CompletedAt = unit.CompletedAt!.Value,
                ExerciseCount = unit.ExerciseList.Count,
                SetCount = unit.ExerciseList.Sum(exercise => exercise.SetResults.Count),
            }).ToList(),
        };
    }

    public async Task<TrainingUnitDto> GetUnitAsync(int unitId)
    {
        var authorId = RequireUserId();
        var unit = await _context.TrainingUnits
            .AsNoTracking()
            .Where(item => item.Id == unitId && item.TrainingPlan.AuthorId == authorId)
            .Include(item => item.ExerciseList)
            .ThenInclude(exercise => exercise.SetResults)
            .SingleOrDefaultAsync();

        return unit is null
            ? throw new KeyNotFoundException("Training unit was not found.")
            : MapUnit(unit);
    }

    public async Task<TrainingUnitDto> CompleteUnitAsync(int unitId, CompleteTrainingUnitDto request)
    {
        var authorId = RequireUserId();
        var unit = await _context.TrainingUnits
            .Where(item => item.Id == unitId && item.TrainingPlan.AuthorId == authorId)
            .Include(item => item.ExerciseList)
            .ThenInclude(exercise => exercise.SetResults)
            .SingleOrDefaultAsync()
            ?? throw new KeyNotFoundException("Training unit was not found.");

        if (request.Exercises.Select(item => item.ExerciseId).Distinct().Count() != unit.ExerciseList.Count
            || request.Exercises.Any(item => unit.ExerciseList.All(exercise => exercise.Id != item.ExerciseId)))
            throw new ArgumentException("Provide results for every exercise in this training unit.");

        foreach (var exercise in unit.ExerciseList)
        {
            var completedExercise = request.Exercises.Single(item => item.ExerciseId == exercise.Id);
            if (completedExercise.Sets.Count != exercise.Sets
                || completedExercise.Sets.Select(set => set.SetNumber).Distinct().Count() != exercise.Sets)
                throw new ArgumentException($"Provide exactly {exercise.Sets} set results for {exercise.Name}.");

            if (exercise.type == ExerciseType.Repetitive
                && completedExercise.Sets.Any(set => set.Repetitions is null))
                throw new ArgumentException($"Repetitions are required for every set of {exercise.Name}.");

            if (exercise.type == ExerciseType.Timed
                && completedExercise.Sets.Any(set => set.Duration is null))
                throw new ArgumentException($"Duration is required for every set of {exercise.Name}.");

            _context.ExerciseSetResults.RemoveRange(exercise.SetResults);
            exercise.SetResults = completedExercise.Sets
                .OrderBy(set => set.SetNumber)
                .Select(set => new ExerciseSetResult
                {
                    SetNumber = set.SetNumber,
                    Weight = set.Weight,
                    Repetitions = set.Repetitions,
                    Duration = set.Duration,
                    Rpe = set.Rpe,
                }).ToList();
        }

        unit.Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim();
        unit.CompletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return MapUnit(unit);
    }

    private int RequireUserId() => _httpContext.GetUserIdFromToken()
        ?? throw new UnauthorizedAccessException("The user id is missing from the token.");

    private static TrainingPlanDto Map(TrainingPlan plan) => new()
    {
        Id = plan.Id,
        Name = plan.Name,
        From = plan.From,
        To = plan.To,
        TrainingUnits = plan.TrainingUnitList.OrderBy(unit => unit.StartTime).Select(MapUnit).ToList(),
    };

    private static TrainingUnitDto MapUnit(TrainingUnit unit) => new()
    {
        Id = unit.Id,
        Name = unit.Name,
        StartTime = unit.StartTime,
        CompletedAt = unit.CompletedAt,
        Notes = unit.Notes,
        Exercises = unit.ExerciseList.Select(exercise => new ExerciseDto
        {
            Id = exercise.Id,
            Name = exercise.Name,
            Description = exercise.Description,
            Sets = exercise.Sets,
            Repetitions = exercise.Repetitions,
            Duration = exercise.Duration,
            Type = exercise.type,
            Tempo = exercise.Tempo is null ? null : string.Join('-', exercise.Tempo),
            SetResults = exercise.SetResults.OrderBy(result => result.SetNumber).Select(result => new ExerciseSetResultDto
            {
                SetNumber = result.SetNumber,
                Weight = result.Weight,
                Repetitions = result.Repetitions,
                Duration = result.Duration,
                Rpe = result.Rpe,
            }).ToList(),
        }).ToList(),
    };
}
