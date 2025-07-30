using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponseDto>> GetAllDepartments();
        Task<Department> createDepartment(DepartmentDto departmentDto);
        Task<ValidationResponseDto> updateDepartment(int id, DepartmentDto departmentDto);
    }
}