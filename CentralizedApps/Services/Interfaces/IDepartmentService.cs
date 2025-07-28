using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponseDto>> GetAllDepartments();
        Task<Department> createDepartment(DepartmentDto departmentDto);
        Department updateDepartment(Department department, DepartmentDto departmentDto);
    }
}