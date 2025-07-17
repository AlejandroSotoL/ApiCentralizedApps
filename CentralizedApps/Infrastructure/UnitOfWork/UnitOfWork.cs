using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Domain.Interfaces;
using CentralizedApps.Infrastructure.Data;
using CentralizedApps.Infrastructure.Repositories;

namespace CentralizedApps.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CentralizedAppsDbContext _context;

        public UnitOfWork(CentralizedAppsDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> genericRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}