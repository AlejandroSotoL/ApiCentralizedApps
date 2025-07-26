using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Data;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;

namespace CentralizedApps.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(CentralizedAppsDbContext Context) : base(Context)
        {

        }
    }
}