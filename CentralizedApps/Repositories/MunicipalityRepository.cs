using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Data;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Repositories
{
    public class MunicipalityRepository : GenericRepository<Municipality>, IMunicipality
    {
        private readonly CentralizedAppsDbContext _Context;
        public MunicipalityRepository(CentralizedAppsDbContext context) : base(context)
        {
            _Context = context;
        }

        public async Task<bool> AddMunicipalityAsync(RegisterMunicipalityDto dto)
        {
            throw new NotImplementedException();
        }
    }
}