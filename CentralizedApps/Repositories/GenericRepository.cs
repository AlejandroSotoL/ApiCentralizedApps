using System.Linq.Expressions;
using CentralizedApps.Data;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly CentralizedAppsDbContext _Context;
        private readonly DbSet<T> _DBset;

        public GenericRepository(CentralizedAppsDbContext Context)
        {
            _Context = Context;
            _DBset = _Context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
                await _DBset.AddAsync(entity);

        }

        public void Delete(T entity)
        {
            
                _DBset.Remove(entity);
            
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
        
            return await _DBset.ToListAsync();
        }

        public async Task<User?> GetByEmailUserByAuthenticate(string email)
        {
        
                return await _Context.Users
                    .FirstOrDefaultAsync(x => x.Email == email);
            
            
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            
                return await _DBset.FindAsync(id);
        }

        public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> filter)
        {
            return await _DBset.FindAsync(filter);
        }

        public void Update(T entity)
        {
                _DBset.Update(entity);
        }
    }
}