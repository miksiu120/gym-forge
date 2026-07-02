using Microsoft.AspNetCore.Identity;
using WorkPlanner.Application.Abstractions;
using WorkPlanner.Application.Exceptions;
using WorkPlanner.Entities;
using WorkPlanner.Models;
using WorkPlanner.Services;

namespace WorkPlanner.Application.Accounts;

public sealed class LoginAccount(
    IAccountRepository accounts,
    IPasswordHasher<User> passwordHasher,
    ITokenService tokenService)
{
    public async Task<LoginResultDto> ExecuteAsync(
        LoginUserDto request,
        CancellationToken cancellationToken = default)
    {
        var user = await accounts.FindByNicknameAsync(
            request.Nickname.Trim(),
            cancellationToken);

        if (user is null
            || passwordHasher.VerifyHashedPassword(user, user.HashedPassword, request.Password)
                == PasswordVerificationResult.Failed)
        {
            throw new InvalidCredentialsException("Invalid nickname or password.");
        }

        return new LoginResultDto
        {
            RefreshToken = tokenService.GenerateRefreshToken(user),
            Token = tokenService.GenerateToken(user),
            Nickname = user.Nickname
        };
    }
}
