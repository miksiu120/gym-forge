using WorkPlanner.Entities;

namespace WorkPlanner.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace PartyGame.Repositories
    {

        public interface IRepository<T> where T : class
        {
            Task<T> CreateAsync(T entity);
            Task<T> GetAsync(int id); 
            Task<IEnumerable<T>> GetAllAsync();
            Task UpdateAsync(T entity);
            Task DeleteAsync(int id); 
        }

        public abstract class Repository<T> : IRepository<T> where T : class
        {
            protected readonly WorkPlannerDbContext _context;
            protected readonly DbSet<T> _dbSet;

            public Repository(WorkPlannerDbContext context)
            {
                _context = context;
                _dbSet = context.Set<T>();
            }

            public async Task<T> CreateAsync(T entity)
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }

            public async Task<T> GetAsync(int id)  
            {
                return await _dbSet.FindAsync(id); 
            }

            public async Task<IEnumerable<T>> GetAllAsync()
            {
                return await _dbSet.ToListAsync();
            }

            public async Task UpdateAsync(T entity)
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id) 
            {
                var entity = await GetAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }

}
