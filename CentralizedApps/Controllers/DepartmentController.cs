using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
        public async Task<IActionResult> createDepartment([FromBody] DepartmentDto departmentDto)
        {

            try
            {
                await _departmentService.createDepartment(departmentDto);
                await _unitOfWork.SaveChangesAsync();
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
        public async Task<IActionResult> updateDepartment(int id, [FromBody] DepartmentDto departmentDto)
        {


            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "NotFound"
                });
            }

            _departmentService.updateDepartment(department, departmentDto);
            await _unitOfWork.SaveChangesAsync();

            return Ok(
                new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 200,
                    SentencesError = ""
                }
            );

        }

    }
}