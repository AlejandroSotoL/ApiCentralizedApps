using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.RemidersDto;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using CentralizedApps.Services.ServicesWeb.Interface;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Services.ServicesWeb
{
    public class GeneralProcedures : IGeneralProcedures
    {
        private readonly IMunicipalityServices _MunicipalityServices;
        private readonly IProcedureServices _ProcedureServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GeneralProcedures(IMunicipalityServices MunicipalityServices, IProcedureServices ProcedureServices, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _MunicipalityServices = MunicipalityServices;
            _ProcedureServices = ProcedureServices;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<MunicipalityProcedureDto_Reminders>> MunicipalityProcedures(int id)
        {
            try
            {
                var response = await _unitOfWork
                    .genericRepository<MunicipalityProcedure>()
                    .GetAllWithNestedIncludesAsync(query =>
                        query.Include(r => r.Municipality)
                                .Include(r => r.Procedures)
                                .Where(q => q.Municipality.Id == id)
                    );

                return _mapper.Map<List<MunicipalityProcedureDto_Reminders>>(response);
            }
            catch (Exception)
            {
                return new List<MunicipalityProcedureDto_Reminders>();
            }
        }
    }
}