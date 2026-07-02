using WorkPlanner.Entities;
using WorkPlanner.Models;

namespace WorkPlanner.Application.TrainingPlans;

public static class TrainingStatisticsCalculator
{
    public static TrainingStatisticsDto Calculate(IReadOnlyList<TrainingUnit> completedUnits)
    {
        var results = completedUnits
            .SelectMany(unit => unit.ExerciseList)
            .SelectMany(exercise => exercise.SetResults)
            .ToList();

        return new TrainingStatisticsDto
        {
            CompletedSessions = completedUnits.Count,
            CompletedSets = results.Count,
            TotalVolume = CalculateVolume(results),
            LastCompletedAt = completedUnits.FirstOrDefault()?.CompletedAt,
            WeightProgress = CalculateWeightProgress(completedUnits),
            RecentSessions = completedUnits.Take(6).Select(unit => new TrainingStatisticsSessionDto
            {
                PlanName = unit.TrainingPlan.Name,
                TrainingName = unit.Name,
                CompletedAt = unit.CompletedAt!.Value,
                ExerciseCount = unit.ExerciseList.Count,
                SetCount = unit.ExerciseList.Sum(exercise => exercise.SetResults.Count)
            }).ToList()
        };
    }

    public static decimal CalculateVolume(IEnumerable<ExerciseSetResult> results) =>
        results
            .Where(result => result.Weight.HasValue && result.Repetitions.HasValue)
            .Sum(result => result.Weight!.Value * result.Repetitions!.Value);

    private static List<TrainingWeightProgressDto> CalculateWeightProgress(
        IEnumerable<TrainingUnit> completedUnits) =>
        completedUnits
            .SelectMany(unit => unit.ExerciseList.Select(exercise => new
            {
                Exercise = exercise.Name,
                CompletedAt = unit.CompletedAt!.Value,
                Weight = exercise.SetResults
                    .Where(result => result.Weight.HasValue)
                    .Select(result => result.Weight!.Value)
                    .DefaultIfEmpty()
                    .Max()
            }))
            .Where(item => item.Weight > 0)
            .GroupBy(item => item.Exercise, StringComparer.OrdinalIgnoreCase)
            .OrderBy(group => group.Key)
            .Select(group => new TrainingWeightProgressDto
            {
                ExerciseName = group.Key,
                Points = group
                    .OrderBy(item => item.CompletedAt)
                    .Select(item => new TrainingWeightPointDto
                    {
                        CompletedAt = item.CompletedAt,
                        Weight = item.Weight
                    })
                    .ToList()
            })
            .ToList();
}
