using CentralizedApps.Data;
using CentralizedApps.Repositories.Interfaces;

namespace CentralizedApps.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
    private readonly CentralizedAppsDbContext _context;
    private IAuthRepository _AuthRepository;
    private IUserRepository _UserRepository;
    private IDepartmentRepository _DepartmentRepository;
    

    public UnitOfWork(CentralizedAppsDbContext context)
        {
            _context = context;
        }

    public IAuthRepository AuthRepositoryUnitOfWork => _AuthRepository ??= new AuthRepository(_context);
    public IUserRepository UserRepository => _UserRepository ??= new UserRepository(_context);

        public IDepartmentRepository DepartmentRepository => _DepartmentRepository ??= new DepartmentRepository(_context);

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