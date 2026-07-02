using FluentValidation;
using WorkPlanner.Enums;
using WorkPlanner.Models;

namespace WorkPlanner.Api.Validation;

public sealed class CreateTrainingPlanDtoValidator : AbstractValidator<CreateTrainingPlanDto>
{
    public CreateTrainingPlanDtoValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(request => request.From)
            .NotEqual(default(DateTimeOffset));
        RuleFor(request => request.To)
            .NotEqual(default(DateTimeOffset));
        RuleFor(request => request.TrainingUnits)
            .NotEmpty();
        RuleForEach(request => request.TrainingUnits)
            .SetValidator(new CreateTrainingUnitDtoValidator());
    }
}

public sealed class CreateTrainingUnitDtoValidator : AbstractValidator<CreateTrainingUnitDto>
{
    public CreateTrainingUnitDtoValidator()
    {
        RuleFor(unit => unit.Name)
            .NotEmpty()
            .MaximumLength(80);
        RuleFor(unit => unit.StartTime)
            .NotEqual(default(DateTimeOffset));
        RuleFor(unit => unit.Exercises)
            .NotEmpty();
        RuleForEach(unit => unit.Exercises)
            .SetValidator(new CreateExerciseDtoValidator());
    }
}

public sealed class CreateExerciseDtoValidator : AbstractValidator<CreateExerciseDto>
{
    public CreateExerciseDtoValidator()
    {
        RuleFor(exercise => exercise.Name)
            .NotEmpty()
            .MaximumLength(80);
        RuleFor(exercise => exercise.Description)
            .MaximumLength(150);
        RuleFor(exercise => exercise.Sets)
            .InclusiveBetween(1, 50);
        RuleFor(exercise => exercise.Type)
            .IsInEnum();
        RuleFor(exercise => exercise.Repetitions)
            .GreaterThan(0)
            .When(exercise => exercise.Repetitions.HasValue);
        RuleFor(exercise => exercise.Duration)
            .InclusiveBetween(1, 86_400)
            .When(exercise => exercise.Duration.HasValue);
        RuleFor(exercise => exercise.Repetitions)
            .NotNull()
            .When(exercise => exercise.Type == ExerciseType.Repetitive)
            .WithMessage("Repetitions are required for repetitive exercises.");
        RuleFor(exercise => exercise.Duration)
            .NotNull()
            .When(exercise => exercise.Type == ExerciseType.Timed)
            .WithMessage("Duration is required for timed exercises.");
        RuleFor(exercise => exercise.Tempo)
            .Must(BeValidTempo)
            .When(exercise => !string.IsNullOrWhiteSpace(exercise.Tempo))
            .WithMessage("Tempo must contain four non-negative integers separated by hyphens.");
    }

    private static bool BeValidTempo(string? tempo) =>
        tempo is not null
        && tempo.Split('-') is { Length: 4 } values
        && values.All(value => int.TryParse(value, out var number) && number >= 0);
}

public sealed class CompleteTrainingUnitDtoValidator : AbstractValidator<CompleteTrainingUnitDto>
{
    public CompleteTrainingUnitDtoValidator()
    {
        RuleFor(request => request.Notes)
            .MaximumLength(1000);
        RuleFor(request => request.Exercises)
            .NotEmpty();
        RuleForEach(request => request.Exercises)
            .SetValidator(new CompleteExerciseDtoValidator());
    }
}

public sealed class CompleteExerciseDtoValidator : AbstractValidator<CompleteExerciseDto>
{
    public CompleteExerciseDtoValidator()
    {
        RuleFor(exercise => exercise.ExerciseId)
            .GreaterThan(0);
        RuleFor(exercise => exercise.Sets)
            .NotEmpty();
        RuleForEach(exercise => exercise.Sets)
            .SetValidator(new CompleteSetDtoValidator());
    }
}

public sealed class CompleteSetDtoValidator : AbstractValidator<CompleteSetDto>
{
    public CompleteSetDtoValidator()
    {
        RuleFor(set => set.SetNumber)
            .InclusiveBetween(1, 50);
        RuleFor(set => set.Weight)
            .InclusiveBetween(0, 2000)
            .When(set => set.Weight.HasValue);
        RuleFor(set => set.Repetitions)
            .InclusiveBetween(0, 1000)
            .When(set => set.Repetitions.HasValue);
        RuleFor(set => set.Duration)
            .InclusiveBetween(0, 86_400)
            .When(set => set.Duration.HasValue);
        RuleFor(set => set.Rpe)
            .InclusiveBetween(0, 10)
            .When(set => set.Rpe.HasValue);
    }
}
