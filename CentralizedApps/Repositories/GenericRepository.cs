using System.Linq.Expressions;
using CentralizedApps.Data;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        protected readonly CentralizedAppsDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(CentralizedAppsDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.FirstOrDefaultAsync(predicate);

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public Task<User?> GetByEmailUserByAuthenticate(string email)
        {
            throw new NotImplementedException();
        }
    }
}