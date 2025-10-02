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

namespace CentralizedApps.Services.ServicesWeb
{
    public class GeneralMunicipality : IGeneralMunicipality
    {
        private readonly IMunicipalityServices _MunicipalityServices;
        private readonly IProcedureServices _ProcedureServices;
        private readonly IDepartmentService _departmentService;
        private readonly IGeneralMunicipality _GeneralMunicipality;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWeb _web;
        public GeneralMunicipality(IMunicipalityServices MunicipalityServices, IProcedureServices ProcedureServices, IUnitOfWork unitOfWork, IDepartmentService departmentService, IWeb web)
        {
            _MunicipalityServices = MunicipalityServices;
            _ProcedureServices = ProcedureServices;
            _unitOfWork = unitOfWork;
            _departmentService = departmentService;
            _web = web;
        }
        public async Task<ValidationResponseDto> UpdateMuniciaplityTransaction(int id, MunicipalityDto dto)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var repo = _unitOfWork.genericRepository<Municipality>();

                var entity = await repo.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "Municipio no encontrado"
                    };
                }

                entity.Name = dto.municipality?.Name;
                entity.EntityCode = dto.municipality?.EntityCode;
                entity.IsActive = dto.municipality?.IsActive;
                entity.Domain = dto.municipality?.Domain;
                entity.UserFintech = dto.municipality?.UserFintech;
                entity.PasswordFintech = dto.municipality?.PasswordFintech;
                entity.DataPrivacy = dto.municipality?.DataPrivacy;
                entity.DataProcessingPrivacy = dto.municipality?.DataProcessingPrivacy;

                repo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 200,
                    SentencesError = "Municipio actualizado correctamente"
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error al actualizar municipio: {e.Message}"
                };
            }
        }
    }
}