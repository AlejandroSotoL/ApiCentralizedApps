using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Services.Interfaces
{
    public interface IProcedureServices
    {        Task<bool> AddSocialMediaType(SocialMediaTypeDto socialMediaType);
    }
}