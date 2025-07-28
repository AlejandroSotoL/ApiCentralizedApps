using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using Microsoft.Data.SqlClient;

namespace CentralizedApps.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DepartmentResponseDto>> GetAllDepartments()
        {
        
                var listDepartaments = await _unitOfWork.DepartmentRepository.GetAllAsync();

                var listDepartamentsDto = listDepartaments
                    .Select(deparment => new DepartmentResponseDto
                    {
                        Name = deparment.Name,
                        Id = deparment.Id,
                    })
                    .ToList();

                return listDepartamentsDto;          

        }
    }
}