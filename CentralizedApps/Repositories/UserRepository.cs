using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Domain.Entities;
using CentralizedApps.Domain.Interfaces;
using CentralizedApps.Infrastructure.Data;
using CentralizedApps.Infrastructure.Repositories;

namespace CentralizedApps.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CentralizedAppsDbContext Context) : base(Context)
        {
        }
    }
}