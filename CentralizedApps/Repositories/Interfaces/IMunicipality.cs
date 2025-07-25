using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Repositories.Interfaces
{
    public interface IMunicipality : IGenericRepository<Municipality>
    {
        Task<bool> AddMunicipalityAsync(RegisterMunicipalityDto dto);
    }
}