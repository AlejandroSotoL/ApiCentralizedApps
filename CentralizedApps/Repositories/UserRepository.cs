using CentralizedApps.Data;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;

namespace CentralizedApps.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CentralizedAppsDbContext Context) : base(Context)
        {
        }
    }
}