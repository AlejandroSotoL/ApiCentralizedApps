

using System.Linq.Expressions;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Repositories.Interfaces

{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        Task<bool>  Remove(T entity);
        Task<User?> GetByEmailUserByAuthenticate(string email);
    }
}