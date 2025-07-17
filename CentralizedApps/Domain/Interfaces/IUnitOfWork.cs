using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Domain.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IGenericRepository<T> genericRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
}