using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.RemidersDto;

namespace CentralizedApps.Services.ServicesWeb.Interface
{
    public interface IGeneralProcedures
    {
        //Reutilizo este DTO
        Task<List<MunicipalityProcedureDto_Reminders>> MunicipalityProcedures(int id);
    }
}