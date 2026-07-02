using Microsoft.EntityFrameworkCore;
using WorkPlanner.Application.Abstractions;
using WorkPlanner.Entities;

namespace WorkPlanner.Infrastructure.Persistence;

public sealed class AccountRepository(WorkPlannerDbContext database) : IAccountRepository
{
    public Task<User?> FindByNicknameAsync(
        string nickname,
        CancellationToken cancellationToken = default) =>
        database.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Nickname == nickname, cancellationToken);

    public Task<User?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken = default) =>
        database.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

    public Task<User?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default) =>
        database.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Id == id, cancellationToken);

    public Task<User?> GetForUpdateAsync(
        int id,
        CancellationToken cancellationToken = default) =>
        database.Users.SingleOrDefaultAsync(user => user.Id == id, cancellationToken);

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default) =>
        await database.Users.AddAsync(user, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        database.SaveChangesAsync(cancellationToken);
}
