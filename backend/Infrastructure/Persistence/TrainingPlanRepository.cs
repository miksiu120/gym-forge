using Microsoft.EntityFrameworkCore;
using WorkPlanner.Application.Abstractions;
using WorkPlanner.Entities;

namespace WorkPlanner.Infrastructure.Persistence;

public sealed class TrainingPlanRepository(WorkPlannerDbContext database) : ITrainingPlanRepository
{
    public async Task AddAsync(
        TrainingPlan plan,
        CancellationToken cancellationToken = default) =>
        await database.TrainingPlans.AddAsync(plan, cancellationToken);

    public async Task<IReadOnlyList<TrainingPlan>> GetByAuthorAsync(
        int authorId,
        CancellationToken cancellationToken = default) =>
        await database.TrainingPlans
            .AsNoTracking()
            .Where(plan => plan.AuthorId == authorId)
            .Include(plan => plan.TrainingUnitList)
            .ThenInclude(unit => unit.ExerciseList)
            .ThenInclude(exercise => exercise.SetResults)
            .AsSplitQuery()
            .OrderByDescending(plan => plan.From)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TrainingUnit>> GetDueUnitsAsync(
        int authorId,
        DateTime through,
        CancellationToken cancellationToken = default) =>
        await database.TrainingUnits
            .AsNoTracking()
            .Where(unit => unit.TrainingPlan.AuthorId == authorId
                && unit.CompletedAt == null
                && unit.StartTime <= through)
            .Include(unit => unit.TrainingPlan)
            .Include(unit => unit.ExerciseList)
            .ThenInclude(exercise => exercise.SetResults)
            .AsSplitQuery()
            .OrderBy(unit => unit.StartTime)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TrainingUnit>> GetCompletedUnitsAsync(
        int authorId,
        CancellationToken cancellationToken = default) =>
        await database.TrainingUnits
            .AsNoTracking()
            .Where(unit => unit.TrainingPlan.AuthorId == authorId && unit.CompletedAt != null)
            .Include(unit => unit.TrainingPlan)
            .Include(unit => unit.ExerciseList)
            .ThenInclude(exercise => exercise.SetResults)
            .AsSplitQuery()
            .OrderByDescending(unit => unit.CompletedAt)
            .ToListAsync(cancellationToken);

    public Task<TrainingUnit?> GetUnitAsync(
        int unitId,
        int authorId,
        CancellationToken cancellationToken = default) =>
        UnitQuery(authorId)
            .AsNoTracking()
            .SingleOrDefaultAsync(unit => unit.Id == unitId, cancellationToken);

    public Task<TrainingUnit?> GetUnitForUpdateAsync(
        int unitId,
        int authorId,
        CancellationToken cancellationToken = default) =>
        UnitQuery(authorId)
            .SingleOrDefaultAsync(unit => unit.Id == unitId, cancellationToken);

    public void RemoveSetResults(IEnumerable<ExerciseSetResult> results) =>
        database.ExerciseSetResults.RemoveRange(results);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        database.SaveChangesAsync(cancellationToken);

    private IQueryable<TrainingUnit> UnitQuery(int authorId) =>
        database.TrainingUnits
            .Where(unit => unit.TrainingPlan.AuthorId == authorId)
            .Include(unit => unit.ExerciseList)
            .ThenInclude(exercise => exercise.SetResults)
            .AsSplitQuery();
}
