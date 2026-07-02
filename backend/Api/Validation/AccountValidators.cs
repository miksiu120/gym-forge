using FluentValidation;
using WorkPlanner.Models;

namespace WorkPlanner.Api.Validation;

public sealed class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(request => request.Nickname)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(request => request.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(160);
        RuleFor(request => request.Name)
            .MaximumLength(80);
        RuleFor(request => request.Surname)
            .MaximumLength(80);
        RuleFor(request => request.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(128);
        RuleFor(request => request.ConfirmPassword)
            .Equal(request => request.Password)
            .WithMessage("Passwords are not the same.");
    }
}

public sealed class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(request => request.Nickname)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(request => request.Password)
            .NotEmpty()
            .MaximumLength(128);
    }
}

public sealed class UpdateAccountDtoValidator : AbstractValidator<UpdateAccountDto>
{
    public UpdateAccountDtoValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(160);
        RuleFor(request => request.Name)
            .MaximumLength(80);
        RuleFor(request => request.Surname)
            .MaximumLength(80);
        RuleFor(request => request.Weight)
            .InclusiveBetween(1, 500)
            .When(request => request.Weight.HasValue);
        RuleFor(request => request.Height)
            .InclusiveBetween(50, 300)
            .When(request => request.Height.HasValue);
        RuleFor(request => request.BirthDay)
            .LessThan(DateTime.UtcNow.Date)
            .When(request => request.BirthDay.HasValue);
        RuleFor(request => request.Description)
            .MaximumLength(500);
        RuleFor(request => request.MeasurementSystem)
            .IsInEnum();
    }
}
