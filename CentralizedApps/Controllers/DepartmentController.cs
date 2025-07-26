using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
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
            try
            {
                
                var listDepartaments = await _departmentService.GetAllDepartments();
                return Ok(listDepartaments);
            }
            catch (SqlException ex) when (ex.Message.Contains("network-related"))
            {
                return StatusCode(500, new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = "Conexión fallida: No se puede establecer conexión con el servidor SQL, inténtalo más tarde."
                });
            }
            catch (InvalidOperationException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 4060)
            {
                return StatusCode(500, new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = "Error: No se puede abrir la base de datos. Verifique permisos o existencia."
                });
            
            }
            catch (InvalidOperationException ex) when (ex.InnerException is SqlException sql && sql.Number == 18456)
            {
                return StatusCode(500, new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = "Error: Fallo en inicio de sesión a la base de datos. Verifique usuario o contraseña."
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = "Internal server error: Error de conexión a la base de datos. Contacte al administrador."
                });
            }
                    
                }

            }
}