

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CentralizedApps.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository AuthRepositoryUnitOfWork { get; }
        IUserRepository UserRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }

        IPaymentHistoryRepository paymentHistoryRepository{ get; }


        IGenericRepository<T> genericRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
        IExecutionStrategy GetExecutionStrategy();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CompleteAsync();
    }
}