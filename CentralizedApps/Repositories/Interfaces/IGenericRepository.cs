

using CentralizedApps.Models.Entities;

namespace CentralizedApps.Repositories.Interfaces

{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        void AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<User?> GetByEmailUserByAuthenticate(string email);
    }
}