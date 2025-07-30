using CentralizedApps.Data;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CentralizedApps.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
    private readonly CentralizedAppsDbContext _context;
    private IAuthRepository _AuthRepository;
    private IUserRepository _UserRepository;
    private IPaymentHistoryRepository _paymentHistoryRepository;
    

    

        public UnitOfWork(CentralizedAppsDbContext context)
        {
            _context = context;
        }

        public IAuthRepository AuthRepositoryUnitOfWork => _AuthRepository ??= new AuthRepository(_context);

        public IUserRepository UserRepository => _UserRepository ??= new UserRepository(_context);

        public IPaymentHistoryRepository paymentHistoryRepository => _paymentHistoryRepository ??= new PaymentHistoryRepository(_context);

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

        public async Task<IDbContextTransaction> BeginTransactionAsync()
            => await _context.Database.BeginTransactionAsync();

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public IExecutionStrategy GetExecutionStrategy()
            => _context.Database.CreateExecutionStrategy();
    }
}