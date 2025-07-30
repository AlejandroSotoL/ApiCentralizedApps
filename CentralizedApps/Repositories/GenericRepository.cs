using System.Linq.Expressions;
using CentralizedApps.Data;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
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

        //Predication
        public async Task<T?> FindAsync_Predicate(Expression<Func<T, bool>> predicate)
        {
            return await _DBset.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {

            return await _DBset.ToListAsync();
        }

        //Predication
        public async Task<List<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _Context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAllWithNestedIncludesAsync(Func<IQueryable<T>, IQueryable<T>> includeFunc)
        {
            IQueryable<T> query = _Context.Set<T>();
            query = includeFunc(query);
            return await query.ToListAsync();
        }

        public async Task<T?> GetOneWithNestedIncludesAsync(
            Func<IQueryable<T>, IQueryable<T>> includeFunc,
            Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _Context.Set<T>();
            query = includeFunc(query);
            return await query.FirstOrDefaultAsync(predicate);
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