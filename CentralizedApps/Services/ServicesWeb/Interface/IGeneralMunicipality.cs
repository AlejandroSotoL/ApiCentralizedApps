using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;

namespace CentralizedApps.Services.ServicesWeb.Interface
{
    public interface IGeneralMunicipality
    {
        Task<ValidationResponseDto> UpdateMuniciaplityTransaction(int id, MunicipalityDto dto);
    }
}