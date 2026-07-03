using WorkPlanner.Application.TrainingPlans;
using WorkPlanner.Entities;

namespace WorkPlanner.Tests;

public sealed class TrainingStatisticsCalculatorTests
{
    [Fact]
    public void Calculate_returns_volume_counts_and_weight_progress()
    {
        var plan = new TrainingPlan { Name = "Strength" };
        var older = CompletedUnit(
            plan,
            "Session A",
            new DateTime(2026, 7, 20, 18, 0, 0, DateTimeKind.Utc),
            [Result(1, 100m, 5), Result(2, 105m, 5)]);
        var newer = CompletedUnit(
            plan,
            "Session B",
            new DateTime(2026, 7, 22, 18, 0, 0, DateTimeKind.Utc),
            [Result(1, 107.5m, 4)]);

        var result = TrainingStatisticsCalculator.Calculate([newer, older]);

        Assert.Equal(2, result.CompletedSessions);
        Assert.Equal(3, result.CompletedSets);
        Assert.Equal(1_455m, result.TotalVolume);
        Assert.Equal(newer.CompletedAt, result.LastCompletedAt);
        var progress = Assert.Single(result.WeightProgress);
        Assert.Equal("Back squat", progress.ExerciseName);
        Assert.Equal([105m, 107.5m], progress.Points.Select(point => point.Weight));
    }

    private static TrainingUnit CompletedUnit(
        TrainingPlan plan,
        string name,
        DateTime completedAt,
        List<ExerciseSetResult> results) => new()
        {
            Name = name,
            TrainingPlan = plan,
            CompletedAt = completedAt,
            ExerciseList =
        [
            new Exercise
            {
                Name = "Back squat",
                Sets = results.Count,
                SetResults = results
            }
        ]
        };

    private static ExerciseSetResult Result(int number, decimal weight, int repetitions) => new()
    {
        SetNumber = number,
        Weight = weight,
        Repetitions = repetitions
    };
}
