using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using EntityPeopleInvitated = CentralizedApps.Models.Entities.PeopleInvitated;
using CentralizedApps.Services.Interfaces;

namespace CentralizedApps.Services
{
    public class PeopleInvitated : IPeopleInvitated
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MunicipalityServices> _logger;
        public PeopleInvitated(ILogger<MunicipalityServices> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ValidationResponseDto> AddPeopleInvitated(CreatePeopleInvitated createPeopleInvitated)
        {
            try
            {

                var entity = _mapper.Map<EntityPeopleInvitated>(createPeopleInvitated);
                await _unitOfWork.genericRepository<EntityPeopleInvitated>().AddAsync(entity);
                var row = await _unitOfWork.SaveChangesAsync();
                if (row <= 0)
                {
                    return new ValidationResponseDto
                    {
                        SentencesError = $"Tenemos problemas - lineas afectadas {row}",
                        BooleanStatus = false,
                        CodeStatus = 404
                    };
                }
                else
                {
                    return new ValidationResponseDto
                    {
                        SentencesError = $"Invitado agregado lienas afectadas {row}",
                        BooleanStatus = true,
                        CodeStatus = 200
                    };
                }
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    SentencesError = $"Tenemos problemas -> {e.Message}",
                    BooleanStatus = false,
                    CodeStatus = 500
                };
            }
        }
    }
}