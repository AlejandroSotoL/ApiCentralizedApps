

namespace CentralizedApps.Repositories.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IAuthRepository AuthRepositoryUnitOfWork { get; }
        IUserRepository UserRepository { get; }

        IGenericRepository<T> genericRepository<T>() where T : class;

        Task<int> SaveChangesAsync();
    }
}