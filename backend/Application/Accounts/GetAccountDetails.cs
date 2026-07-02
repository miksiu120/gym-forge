using WorkPlanner.Application.Abstractions;
using WorkPlanner.Application.Exceptions;
using WorkPlanner.Models;

namespace WorkPlanner.Application.Accounts;

public sealed class GetAccountDetails(
    IAccountRepository accounts,
    ICurrentUser currentUser)
{
    public async Task<AccountDetailsDto> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        var user = await accounts.GetByIdAsync(
            currentUser.GetRequiredUserId(),
            cancellationToken)
            ?? throw new NotFoundException("Account was not found.");

        return AccountMapper.ToDetails(user);
    }
}
