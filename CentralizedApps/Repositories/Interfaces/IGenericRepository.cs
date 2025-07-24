using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CentralizedApps.Domain.Entities;

namespace CentralizedApps.Domain.Interfaces
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