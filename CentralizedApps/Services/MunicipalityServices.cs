using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;

namespace CentralizedApps.Services
{
    public class MunicipalityServices : IMunicipalityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MunicipalityServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddMunicipalityMasivAsync(RegisterMunicipalityDto dto)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                /*-------------DEPARTAMENT-------------*/
                var departamentRepo = _unitOfWork.genericRepository<Department>();
                var departament = await departamentRepo.FindAsync(d => d.Name == dto.Departamento);
                if (departament == null)
                {
                    departament = new Department { Name = dto.Departamento };
                    await departamentRepo.AddAsync(departament);
                    await _unitOfWork.CompleteAsync();
                }

                /*-------------THEME--------------------*/
                var themeRepo = _unitOfWork.genericRepository<Theme>();
                var theme = await themeRepo.FindAsync(t => t.BackGroundColor == dto.TemaName);
                if (theme == null)
                {
                    // Validar si los campos necesarios vienen en el DTO
                    if (string.IsNullOrWhiteSpace(dto.BackGroundColor) ||
                        string.IsNullOrWhiteSpace(dto.PrimaryColor) ||
                        string.IsNullOrWhiteSpace(dto.SecondaryColor))
                    {
                        throw new Exception("Los datos del tema son requeridos porque no existe un tema con ese nombre.");
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}