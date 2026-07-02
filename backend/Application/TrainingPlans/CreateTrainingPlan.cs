using WorkPlanner.Application.Abstractions;
using WorkPlanner.Application.Exceptions;
using WorkPlanner.Entities;
using WorkPlanner.Enums;
using WorkPlanner.Models;

namespace WorkPlanner.Application.TrainingPlans;

public sealed class CreateTrainingPlan(
    ITrainingPlanRepository trainingPlans,
    ICurrentUser currentUser)
{
    public async Task<TrainingPlanDto> ExecuteAsync(
        CreateTrainingPlanDto request,
        CancellationToken cancellationToken = default)
    {
        var from = request.From.UtcDateTime;
        var to = request.To.UtcDateTime;

        if (from > to)
        {
            throw new BadRequestException(
                "The plan end date cannot be earlier than its start date.");
        }

        if (request.TrainingUnits.Any(unit =>
                unit.StartTime.UtcDateTime < from || unit.StartTime.UtcDateTime > to))
        {
            throw new BadRequestException(
                "Every training unit must be scheduled within the plan date range.");
        }

        var plan = new TrainingPlan
        {
            Name = request.Name.Trim(),
            From = from,
            To = to,
            AuthorId = currentUser.GetRequiredUserId(),
            TrainingUnitList = request.TrainingUnits.Select(unit => new TrainingUnit
            {
                Name = unit.Name.Trim(),
                StartTime = unit.StartTime.UtcDateTime,
                ExerciseList = unit.Exercises.Select(exercise => new Exercise
                {
                    Name = exercise.Name.Trim(),
                    Description = NormalizeOptional(exercise.Description),
                    Sets = exercise.Sets,
                    Repetitions = exercise.Type == ExerciseType.Repetitive
                        ? exercise.Repetitions
                        : null,
                    Duration = exercise.Type == ExerciseType.Timed
                        ? exercise.Duration
                        : null,
                    type = exercise.Type,
                    Tempo = ParseTempo(exercise.Tempo)
                }).ToList()
            }).ToList()
        };

        await trainingPlans.AddAsync(plan, cancellationToken);
        await trainingPlans.SaveChangesAsync(cancellationToken);
        return TrainingPlanMapper.ToResponse(plan);
    }

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static int[]? ParseTempo(string? tempo) =>
        string.IsNullOrWhiteSpace(tempo)
            ? null
            : tempo.Split('-').Select(int.Parse).ToArray();
}
