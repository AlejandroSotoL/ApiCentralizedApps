using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponseDto>> GetAllDepartments();
        Task<Department> createDepartment(CreateDepartmentDto departmentDto);
        Task<ValidationResponseDto> updateDepartment(int id, CreateDepartmentDto departmentDto);
    }
}