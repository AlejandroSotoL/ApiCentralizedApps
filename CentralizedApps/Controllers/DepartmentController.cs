using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CentralizedApps.Controllers
{
    [ApiController]
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

        [HttpPost]
        public async Task<IActionResult> createDepartment([FromBody] CreateDepartmentDto departmentDto)
        {

            try
            {
                if (departmentDto == null)
                {
                    return BadRequest(
                    new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = "el objeto no puede ser null"
                    });
                }
                await _departmentService.createDepartment(departmentDto);
                return Ok(
                    new ValidationResponseDto
                    {
                        BooleanStatus = true,
                        CodeStatus = 200,
                        SentencesError = ""
                    }
                );
                
            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = $"Error: {ex.Message}"
                });
            }

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> updateDepartment(int id, [FromBody] CreateDepartmentDto updatedepartmentDto)
        {


            try
            {

                if (updatedepartmentDto == null)
                {
                    return BadRequest(
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
                        return BadRequest(new ValidationResponseDto
                        {
                            BooleanStatus = result.BooleanStatus,
                            CodeStatus = result.CodeStatus,
                            SentencesError = "Error: " + result.SentencesError
                        });
                    }
                    else
                    {
                        return Ok(new ValidationResponseDto
                        {
                            BooleanStatus = result.BooleanStatus,
                            CodeStatus = result.CodeStatus,
                            SentencesError = result.SentencesError
                        });
                    }
            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Error: " + ex.Message
                });
            }

        }

    }
}