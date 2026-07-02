using WorkPlanner.Application.Abstractions;
using WorkPlanner.Application.Exceptions;
using WorkPlanner.Models;

namespace WorkPlanner.Application.Accounts;

public sealed class UpdateAccount(
    IAccountRepository accounts,
    ICurrentUser currentUser)
{
    public async Task<AccountDetailsDto> ExecuteAsync(
        UpdateAccountDto request,
        CancellationToken cancellationToken = default)
    {
        var user = await accounts.GetForUpdateAsync(
            currentUser.GetRequiredUserId(),
            cancellationToken)
            ?? throw new NotFoundException("Account was not found.");

        var email = request.Email.Trim();
        var owner = await accounts.FindByEmailAsync(email, cancellationToken);
        if (owner is not null && owner.Id != user.Id)
        {
            throw new ConflictException("An account with this email already exists.");
        }

        user.Email = email;
        user.Name = NormalizeOptional(request.Name);
        user.Surname = NormalizeOptional(request.Surname);
        user.Weight = request.Weight;
        user.Height = request.Height;
        user.BirthDay = request.BirthDay.HasValue
            ? DateTime.SpecifyKind(request.BirthDay.Value, DateTimeKind.Utc)
            : null;
        user.Description = NormalizeOptional(request.Description);
        user.MeasurementSystem = request.MeasurementSystem;

        await accounts.SaveChangesAsync(cancellationToken);
        return AccountMapper.ToDetails(user);
    }

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
