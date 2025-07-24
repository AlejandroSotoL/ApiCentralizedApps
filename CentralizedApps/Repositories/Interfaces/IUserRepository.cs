using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Domain.Entities;

namespace CentralizedApps.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        
    }
}