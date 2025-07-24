

using CentralizedApps.Models.Entities;

namespace CentralizedApps.Repositories.Interfaces

{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<string> AddAsync(T entity);
        string Update(T entity);
        string Delete(T entity);
        Task<User?> GetByEmailUserByAuthenticate(string email);
    }
}