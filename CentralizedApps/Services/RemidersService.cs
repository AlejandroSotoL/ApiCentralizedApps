using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.RemidersDto;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Services
{
    public class RemidersService : IRemidersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MunicipalityServices> _logger;
        public RemidersService(ILogger<MunicipalityServices> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<ValidationResponseDto> createReminders(CreateReminderDto createReminderDto)
        {
            try
            {
                var reminder = _mapper.Map<Reminder>(createReminderDto);
                await _unitOfWork.genericRepository<Reminder>().AddAsync(reminder);
                await _unitOfWork.SaveChangesAsync();

                return new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 201,
                    SentencesError = "Recordatorio creado correctamente."
                };
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error al crear el recordatorio: {ex.Message}"
                };
            }
        }

        public async Task<List<ResponseReminderDto>> GetReminders()
        {
            try
            {

                var entities = await _unitOfWork.genericRepository<Reminder>().GetAllWithNestedIncludesAsync(query =>
                    query
                        .Include(r => r.IdProcedureMunicipalityNavigation)!
                            .ThenInclude(pm => pm.Procedures)!
                        .Include(r => r.IdUserNavigation)!
                        .Include(r => r.IdProcedureMunicipalityNavigation)
                            .ThenInclude(pm => pm.Municipality)
                );

                if (entities == null || !entities.Any())
                    return new List<ResponseReminderDto>();
                return _mapper.Map<List<ResponseReminderDto>>(entities);
            }
            catch (Exception)
            {
                return new List<ResponseReminderDto>();
            }
        }

        public async Task<List<ResponseReminderDto>> GetRemindersByUserId(int userId)
        {
            try
            {
                var entities = await _unitOfWork.genericRepository<Reminder>().GetAllWithNestedIncludesAsync(query =>
                    query
                                .Where(r => r.IdUser == userId)
                                .Include(r => r.IdProcedureMunicipalityNavigation)
                                    .ThenInclude(pm => pm.Procedures)
                                .Include(r => r.IdProcedureMunicipalityNavigation)
                                    .ThenInclude(pm => pm.Municipality)
                                .Include(r => r.IdUserNavigation)
                        );

                if (entities == null || !entities.Any())
                    return new List<ResponseReminderDto>();

                return _mapper.Map<List<ResponseReminderDto>>(entities);
            }
            catch (Exception)
            {
                return new List<ResponseReminderDto>();
            }
        }
    }
}