using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;

namespace CentralizedApps.Services.ServicesWeb.Interface
{
    public interface IWeb
    {
        Task<MunicipalitiesAndSocialMediaTypeDto> MunicipalitiesAndSocialMediaType(int? id);
    }
}