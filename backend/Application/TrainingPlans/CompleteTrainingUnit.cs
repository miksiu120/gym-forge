using WorkPlanner.Application.Abstractions;
using WorkPlanner.Application.Exceptions;
using WorkPlanner.Entities;
using WorkPlanner.Enums;
using WorkPlanner.Models;

namespace WorkPlanner.Application.TrainingPlans;

public sealed class CompleteTrainingUnit(
    ITrainingPlanRepository trainingPlans,
    ICurrentUser currentUser)
{
    public async Task<TrainingUnitDto> ExecuteAsync(
        int unitId,
        CompleteTrainingUnitDto request,
        CancellationToken cancellationToken = default)
    {
        var unit = await trainingPlans.GetUnitForUpdateAsync(
            unitId,
            currentUser.GetRequiredUserId(),
            cancellationToken)
            ?? throw new NotFoundException("Training unit was not found.");

        EnsureEveryExerciseHasResults(unit, request);

        foreach (var exercise in unit.ExerciseList)
        {
            var completedExercise = request.Exercises.Single(
                item => item.ExerciseId == exercise.Id);
            EnsureExpectedSets(exercise, completedExercise);
            EnsureRequiredResultValues(exercise, completedExercise);

            trainingPlans.RemoveSetResults(exercise.SetResults);
            exercise.SetResults = completedExercise.Sets
                .OrderBy(set => set.SetNumber)
                .Select(set => new ExerciseSetResult
                {
                    SetNumber = set.SetNumber,
                    Weight = set.Weight,
                    Repetitions = set.Repetitions,
                    Duration = set.Duration,
                    Rpe = set.Rpe
                })
                .ToList();
        }

        unit.Notes = string.IsNullOrWhiteSpace(request.Notes)
            ? null
            : request.Notes.Trim();
        unit.CompletedAt = DateTime.UtcNow;

        await trainingPlans.SaveChangesAsync(cancellationToken);
        return TrainingPlanMapper.ToUnitResponse(unit);
    }

    private static void EnsureEveryExerciseHasResults(
        TrainingUnit unit,
        CompleteTrainingUnitDto request)
    {
        var requestedIds = request.Exercises
            .Select(item => item.ExerciseId)
            .ToList();
        var expectedIds = unit.ExerciseList
            .Select(exercise => exercise.Id)
            .ToHashSet();

        if (requestedIds.Count != expectedIds.Count
            || requestedIds.Distinct().Count() != requestedIds.Count
            || requestedIds.Any(id => !expectedIds.Contains(id)))
        {
            throw new BadRequestException(
                "Provide results for every exercise in this training unit.");
        }
    }

    private static void EnsureExpectedSets(
        Exercise exercise,
        CompleteExerciseDto completedExercise)
    {
        var setNumbers = completedExercise.Sets
            .Select(set => set.SetNumber)
            .Order()
            .ToList();

        if (!setNumbers.SequenceEqual(Enumerable.Range(1, exercise.Sets)))
        {
            throw new BadRequestException(
                $"Provide exactly {exercise.Sets} set results for {exercise.Name}.");
        }
    }

    private static void EnsureRequiredResultValues(
        Exercise exercise,
        CompleteExerciseDto completedExercise)
    {
        if (exercise.type == ExerciseType.Repetitive
            && completedExercise.Sets.Any(set => set.Repetitions is null))
        {
            throw new BadRequestException(
                $"Repetitions are required for every set of {exercise.Name}.");
        }

        if (exercise.type == ExerciseType.Timed
            && completedExercise.Sets.Any(set => set.Duration is null))
        {
            throw new BadRequestException(
                $"Duration is required for every set of {exercise.Name}.");
        }
    }
}
