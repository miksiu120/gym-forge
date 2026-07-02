using WorkPlanner.Entities;

namespace WorkPlanner.Application.Abstractions;

public interface ITrainingPlanRepository
{
    Task AddAsync(
        TrainingPlan plan,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TrainingPlan>> GetByAuthorAsync(
        int authorId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TrainingUnit>> GetDueUnitsAsync(
        int authorId,
        DateTime through,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TrainingUnit>> GetCompletedUnitsAsync(
        int authorId,
        CancellationToken cancellationToken = default);

    Task<TrainingUnit?> GetUnitAsync(
        int unitId,
        int authorId,
        CancellationToken cancellationToken = default);

    Task<TrainingUnit?> GetUnitForUpdateAsync(
        int unitId,
        int authorId,
        CancellationToken cancellationToken = default);

    void RemoveSetResults(IEnumerable<ExerciseSetResult> results);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
