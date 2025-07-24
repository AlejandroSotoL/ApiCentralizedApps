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
        private IAuthRepository _AuthRepository;

        public UnitOfWork(CentralizedAppsDbContext context)
        {
            _context = context;
        }

        //Auth from credentials and more things
        public IAuthRepository AuthRepositoryUnitOfWork => _AuthRepository ??= new AuthRepository(_context);

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