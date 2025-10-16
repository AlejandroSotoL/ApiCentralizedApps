using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace CentralizedApps.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IUnitOfWork unitOfWork, IDepartmentService departmentService)
        {
            _unitOfWork = unitOfWork;
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var listDepartaments = await _departmentService.GetAllDepartments();
            return Ok(listDepartaments);
        }

        [HttpPost("Create")]
        public async Task<ValidationResponseDto> createDepartment([FromBody] CreateDepartmentDto departmentDto)
        {

            try
            {
                if (departmentDto == null)
                {
                    return
                    new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = "el objeto no puede ser null"
                    };
                }
                await _departmentService.createDepartment(departmentDto);
                return
                    new ValidationResponseDto
                    {
                        BooleanStatus = true,
                        CodeStatus = 200,
                        SentencesError = ""
                    };
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = $"Error: {ex.Message}"
                };
            }

        }
        [HttpPut("Edit{id}")]
        public async Task<ValidationResponseDto> updateDepartment(int id, [FromBody] CreateDepartmentDto updatedepartmentDto)
        {
            try
            {
                if (updatedepartmentDto == null)
                {
                    return (
                    new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = "el objeto no puede ser null"
                    });
                }
                var result = await _departmentService.updateDepartment(id, updatedepartmentDto);
                if (!result.BooleanStatus)
                    {
                        return (new ValidationResponseDto
                        {
                            BooleanStatus = result.BooleanStatus,
                            CodeStatus = result.CodeStatus,
                            SentencesError = "Error: " + result.SentencesError
                        });
                    }
                    else
                    {
                        return new ValidationResponseDto
                        {
                            BooleanStatus = result.BooleanStatus,
                            CodeStatus = result.CodeStatus,
                            SentencesError = result.SentencesError
                        };
                    }
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Error: " + ex.Message
                };
            }
        }
    }
}