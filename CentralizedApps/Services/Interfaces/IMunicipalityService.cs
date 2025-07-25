using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;

namespace CentralizedApps.Services.Interfaces
{
    public interface IMunicipalityService
    {
        Task<bool> AddMunicipalityMasivAsync(RegisterMunicipalityDto dto);
    }
}