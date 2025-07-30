using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;


namespace CentralizedApps.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DepartmentResponseDto>> GetAllDepartments()
        {

            var listDepartaments = await _unitOfWork.genericRepository<Department>().GetAllAsync();

            var listDepartamentsDto = listDepartaments
                .Select(deparment => new DepartmentResponseDto
                {
                    Name = deparment.Name,
                    Id = deparment.Id,
                })
                .ToList();

            return listDepartamentsDto;


        }


        public async Task<Department> createDepartment(DepartmentDto departmentDto)
        {
            Department department = new Department
            {
                Id = departmentDto.Id,
                Name = departmentDto.Name
            };


        await _unitOfWork.genericRepository<Department>().AddAsync(department);
        await _unitOfWork.SaveChangesAsync();
            return department;
        }
        

        public async Task<ValidationResponseDto> updateDepartment(int id, DepartmentDto departmentDto)
        {
            var department = await _unitOfWork.genericRepository<Department>().GetByIdAsync(id);
            if (department == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "notfund"
                };
            }
            department.Name = departmentDto.Name;
            _unitOfWork.genericRepository<Department>().Update(department);
            await _unitOfWork.SaveChangesAsync();
            return new ValidationResponseDto
                {
                    CodeStatus = 200,
                    BooleanStatus = true,
                    SentencesError = "succesfully"
                };
        }
    }
}