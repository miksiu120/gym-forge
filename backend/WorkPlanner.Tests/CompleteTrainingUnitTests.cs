using WorkPlanner.Application.Abstractions;
using WorkPlanner.Application.Exceptions;
using WorkPlanner.Application.TrainingPlans;
using WorkPlanner.Entities;
using WorkPlanner.Enums;
using WorkPlanner.Models;

namespace WorkPlanner.Tests;

public sealed class CompleteTrainingUnitTests
{
    [Fact]
    public async Task Execute_rejects_non_contiguous_set_numbers()
    {
        var unit = new TrainingUnit
        {
            Id = 7,
            ExerciseList =
            [
                new Exercise
                {
                    Id = 11,
                    Name = "Back squat",
                    Sets = 2,
                    type = ExerciseType.Repetitive
                }
            ]
        };
        var useCase = new CompleteTrainingUnit(
            new StubTrainingPlanRepository(unit),
            new StubCurrentUser());
        var request = new CompleteTrainingUnitDto
        {
            Exercises =
            [
                new CompleteExerciseDto
                {
                    ExerciseId = 11,
                    Sets =
                    [
                        new CompleteSetDto { SetNumber = 1, Repetitions = 5 },
                        new CompleteSetDto { SetNumber = 3, Repetitions = 5 }
                    ]
                }
            ]
        };

        var exception = await Assert.ThrowsAsync<BadRequestException>(
            () => useCase.ExecuteAsync(unit.Id, request));

        Assert.Contains("exactly 2 set results", exception.Message);
    }

    private sealed class StubCurrentUser : ICurrentUser
    {
        public int GetRequiredUserId() => 42;
    }

    private sealed class StubTrainingPlanRepository(TrainingUnit unit) : ITrainingPlanRepository
    {
        public Task AddAsync(TrainingPlan plan, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;

        public Task<IReadOnlyList<TrainingPlan>> GetByAuthorAsync(
            int authorId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<TrainingPlan>>([]);

        public Task<IReadOnlyList<TrainingUnit>> GetDueUnitsAsync(
            int authorId,
            DateTime through,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<TrainingUnit>>([]);

        public Task<IReadOnlyList<TrainingUnit>> GetCompletedUnitsAsync(
            int authorId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<TrainingUnit>>([]);

        public Task<TrainingUnit?> GetUnitAsync(
            int unitId,
            int authorId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<TrainingUnit?>(unit);

        public Task<TrainingUnit?> GetUnitForUpdateAsync(
            int unitId,
            int authorId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<TrainingUnit?>(unit);

        public void RemoveSetResults(IEnumerable<ExerciseSetResult> results)
        {
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }
}
