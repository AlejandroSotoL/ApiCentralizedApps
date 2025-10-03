using System.Linq.Expressions;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Repositories.Interfaces

{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<User?> GetByEmailUserByAuthenticate(string email);
        Task<List<T>> GetAllWithFilterAsync(Expression<Func<T, bool>> filter);
        void DeleteRange(IEnumerable<T> entities);
        //Predication
        Task<T?> FindAsync_Predicate(Expression<Func<T, bool>> predicate);
        //Predication
        Task<List<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllWithNestedIncludesAsync(
            Func<IQueryable<T>, IQueryable<T>> includeFunc
        );
        Task<T?> GetOneWithNestedIncludesAsync(
            Func<IQueryable<T>, IQueryable<T>> includeFunc,
            Expression<Func<T, bool>> predicate);

    }
}