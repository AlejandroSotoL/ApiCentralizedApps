using CentralizedApps.Data;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CentralizedApps.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CentralizedAppsDbContext _context;
        private IAuthRepository _AuthRepository;
        private IMunicipality _MunicipalityRepository;
        public UnitOfWork(CentralizedAppsDbContext context)
        {
            _context = context;
        }

        public IAuthRepository AuthRepositoryUnitOfWork => _AuthRepository ??= new AuthRepository(_context);

        public IMunicipality MunicipalityUnitOfWork => _MunicipalityRepository ??= new MunicipalityRepository(_context);

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

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}