using WorkPlanner.Entities;

namespace WorkPlanner.Application.Abstractions;

public interface IAccountRepository
{
    Task<User?> FindByNicknameAsync(
        string nickname,
        CancellationToken cancellationToken = default);

    Task<User?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<User?> GetForUpdateAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
