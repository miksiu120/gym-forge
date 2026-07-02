using Microsoft.AspNetCore.Identity;
using WorkPlanner.Application.Abstractions;
using WorkPlanner.Application.Exceptions;
using WorkPlanner.Entities;
using WorkPlanner.Models;

namespace WorkPlanner.Application.Accounts;

public sealed class RegisterAccount(
    IAccountRepository accounts,
    IPasswordHasher<User> passwordHasher)
{
    public async Task ExecuteAsync(
        CreateUserDto request,
        CancellationToken cancellationToken = default)
    {
        var nickname = request.Nickname.Trim();
        var email = request.Email.Trim();

        if (await accounts.FindByEmailAsync(email, cancellationToken) is not null)
        {
            throw new ConflictException("An account with this email already exists.");
        }

        if (await accounts.FindByNicknameAsync(nickname, cancellationToken) is not null)
        {
            throw new ConflictException("An account with this nickname already exists.");
        }

        var user = new User
        {
            Nickname = nickname,
            Email = email,
            Name = NormalizeOptional(request.Name),
            Surname = NormalizeOptional(request.Surname),
            CreatedAt = DateTime.UtcNow
        };

        user.HashedPassword = passwordHasher.HashPassword(user, request.Password);
        await accounts.AddAsync(user, cancellationToken);
        await accounts.SaveChangesAsync(cancellationToken);
    }

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
