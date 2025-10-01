using System;
using Microsoft.AspNetCore.Mvc;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services;
using CentralizedApps.Services.Interfaces;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using Microsoft.AspNetCore.Authorization;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]

    public class ProcessOfMunicipalitiesController : ControllerBase
    {

        private readonly IProcedureServices _ProcedureServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MunicipalityServices> _logger;
        public ProcessOfMunicipalitiesController(ILogger<MunicipalityServices> logger, IProcedureServices ProcedureServices, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _ProcedureServices = ProcedureServices;
        }

        [HttpPost("Asing/Procedure")]
        public async Task<IActionResult> AsingProccessToMunicipality(MunicipalityProcedureAddDto municipalityProcedureAddDto)
        {
            if (municipalityProcedureAddDto == null)
            {
                return BadRequest(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Los datos del procedimiento son requeridos."
                });
            }

            var result = await _ProcedureServices.AsingProccessToMunicipality(municipalityProcedureAddDto);
            return Ok(result);
        }


        [HttpPost("Asing/TypeSocialMedia")]
        public async Task<ValidationResponseDto> AsignSocialMediaToMunicipality([FromBody] MunicipalitySocialMeditaDto_Response dto)
        {
            if (dto == null)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 404,
                    BooleanStatus = false,
                    SentencesError = "Es necesaria la información."
                };
            }

            var result = await _ProcedureServices.AddMuncipalitySocialMediaToMunicipality(dto);
            if (!result)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = "Tenemos problemas al asignar la red social al municipio."
                };
            }
            return new ValidationResponseDto
            {
                CodeStatus = 200,
                BooleanStatus = true,
                SentencesError = "Red social asignada correctamente al municipio."
            };
        }


        [HttpPost("Asing/SportsFacility")]
        public async Task<IActionResult> createSportsFacility([FromBody] CreateSportsFacilityDto createSportsFacilityDto)
        {
            try
            {
                await _ProcedureServices.createSportsFacility(createSportsFacilityDto);

                return Ok(new ValidationResponseDto
                {
                    CodeStatus = 200,
                    BooleanStatus = true,
                    SentencesError = ""
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = ex.Message
                });
            }
        }

        [HttpPost("Create/Asing/Course")]
        public async Task<ValidationResponseDto> createCourse([FromBody] CreateCourseDto createCourseDto)
        {
            try
            {
                await _ProcedureServices.createCourse(createCourseDto);

                return new ValidationResponseDto
                {
                    CodeStatus = 200,
                    BooleanStatus = true,
                    SentencesError = "Curso creado correctamente"
                };
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = ex.Message
                };
            }
        }

        [HttpPut("UpdateMuncipality/{Id}")]
        public async Task<ValidationResponseDto> UpdateMunicipality(int Id, CompleteMunicipalityDto MunicipalityDTO)
        {
            if (MunicipalityDTO == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Los datos del municipio son requeridos."
                };
            }

            var result = await _ProcedureServices.UpdateMunicipality(Id, MunicipalityDTO);
            if (!result.BooleanStatus)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = result.BooleanStatus,
                    CodeStatus = result.CodeStatus,
                    SentencesError = "Error: " + result.SentencesError
                };
            }
            return result;
        }
        [HttpPut("Update/ProcedureStatus/{id}")]
        public async Task<ValidationResponseDto> UpdateProcedureStatus(int id, [FromQuery] bool? status, [FromQuery] string IntegrationType)
        {
            if (id <= 0 || status == null || IntegrationType == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Datos enviados incorrectos."
                };
            }

            try
            {
                var result = await _ProcedureServices.UpdateProcedureStatus(id, status.Value, IntegrationType);
                return new ValidationResponseDto
                {
                    BooleanStatus = result.BooleanStatus,
                    CodeStatus = result.CodeStatus,
                    SentencesError = result.SentencesError
                };
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = "Error inesperado al actualizar el estado del procedimiento."
                };
            }
        }
    }
}