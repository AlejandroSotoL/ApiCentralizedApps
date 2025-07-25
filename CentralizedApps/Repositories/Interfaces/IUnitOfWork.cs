

using Microsoft.EntityFrameworkCore.Storage;

namespace CentralizedApps.Repositories.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IAuthRepository AuthRepositoryUnitOfWork { get; }
        IMunicipality MunicipalityUnitOfWork { get; }
        IGenericRepository<T> genericRepository<T>() where T : class;
        Task<int> CompleteAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> SaveChangesAsync();
    }
}