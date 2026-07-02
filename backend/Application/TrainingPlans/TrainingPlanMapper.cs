using WorkPlanner.Entities;
using WorkPlanner.Models;

namespace WorkPlanner.Application.TrainingPlans;

public static class TrainingPlanMapper
{
    public static TrainingPlanDto ToResponse(TrainingPlan plan) => new()
    {
        Id = plan.Id,
        Name = plan.Name,
        From = plan.From,
        To = plan.To,
        TrainingUnits = plan.TrainingUnitList
            .OrderBy(unit => unit.StartTime)
            .Select(ToUnitResponse)
            .ToList()
    };

    public static TrainingUnitDto ToUnitResponse(TrainingUnit unit) => new()
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
            SetResults = exercise.SetResults
                .OrderBy(result => result.SetNumber)
                .Select(result => new ExerciseSetResultDto
                {
                    SetNumber = result.SetNumber,
                    Weight = result.Weight,
                    Repetitions = result.Repetitions,
                    Duration = result.Duration,
                    Rpe = result.Rpe
                })
                .ToList()
        }).ToList()
    };
}
