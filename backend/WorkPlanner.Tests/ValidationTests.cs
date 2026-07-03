using WorkPlanner.Api.Validation;
using WorkPlanner.Enums;
using WorkPlanner.Models;

namespace WorkPlanner.Tests;

public sealed class ValidationTests
{
    [Fact]
    public async Task CreateUser_rejects_invalid_identity_and_password()
    {
        var validator = new CreateUserDtoValidator();
        var request = new CreateUserDto
        {
            Nickname = "",
            Email = "not-an-email",
            Password = "short",
            ConfirmPassword = "different"
        };

        var result = await validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(request.Nickname));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(request.Email));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(request.Password));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(request.ConfirmPassword));
    }

    [Fact]
    public async Task Timed_exercise_requires_duration_and_valid_tempo()
    {
        var validator = new CreateExerciseDtoValidator();
        var request = new CreateExerciseDto
        {
            Name = "Plank",
            Sets = 3,
            Type = ExerciseType.Timed,
            Tempo = "3-1-invalid-0"
        };

        var result = await validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(request.Duration));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(request.Tempo));
    }
}
