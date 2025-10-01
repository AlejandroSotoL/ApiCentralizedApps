using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using CentralizedApps.Services.ServicesWeb.Interface;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Services.ServicesWeb
{
    public class Web : IWeb
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMunicipalityServices _municipalityServices;

        public Web(IUnitOfWork unitOfWork, IMunicipalityServices municipalityServices)
        {
            _unitOfWork = unitOfWork;
            _municipalityServices = municipalityServices;
        }

        public async Task<MunicipalitiesAndSocialMediaTypeDto> MunicipalitiesAndSocialMediaType(int? id)
        {
            var response = _unitOfWork.genericRepository<MunicipalitySocialMedium>();
            var Entity = await response.GetAllWithNestedIncludesAsync(query =>
                query
                    .Include(msm => msm.Municipality)
                    .Include(msm => msm.SocialMediaType)
            );

            var filtro = id.HasValue
                ? Entity.Where(m => m.MunicipalityId == id.Value).ToList()
                : new List<MunicipalitySocialMedium>();

            return new MunicipalitiesAndSocialMediaTypeDto
            {
                municipality = id.HasValue
                    ? await _municipalityServices.JustGetMunicipalityWithRelations(id.Value)
                    : null,
                municipalities = await _unitOfWork.genericRepository<Municipality>().GetAllAsync(),
                socialMediaTypes = await _unitOfWork.genericRepository<SocialMediaType>().GetAllAsync(),
                municipalitySocialMedia = filtro
            };

        }

    }
}