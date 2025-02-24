using Microsoft.EntityFrameworkCore;
using WorkPlanner.Entities;
using WorkPlanner.Repositories.PartyGame.Repositories;

namespace WorkPlanner.Repositories
{
    public interface IAccountRepository:IRepository<User>
    {
        Task<User> GetAccountByNicknameAsync(string nickname);
        Task<User> GetAccountByEmailAsync(string email );
    }

    public class AccountRepository:Repository<User>, IAccountRepository
    {

        public AccountRepository(WorkPlannerDbContext context):base(context)
        {

        }

        public async Task<User> GetAccountByNicknameAsync(string nickname)
        {
            return await _dbSet.FirstOrDefaultAsync(user => user.Nickname == nickname);
        }

        public async Task<User> GetAccountByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(user => user.Email == email);
        }

    }
}
