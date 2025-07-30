using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Services;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProceduresController : ControllerBase
    {
        private readonly IProcedureServices _ProcedureServices;
        private readonly ILogger<MunicipalityServices> _logger;
        public ProceduresController(ILogger<MunicipalityServices> logger, IProcedureServices ProcedureServices)
        {
            _logger = logger;
            _ProcedureServices = ProcedureServices;
        }

        

        [HttpPost("CursesSports")]
        public async Task<IActionResult> createCurseSports([FromBody] CourseSportsFacilityDto courseSportsFacilityDto)
        {
            try
            {


                await _ProcedureServices.createCurseSports(courseSportsFacilityDto);
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


        [HttpPost("DocumentType")]
        public async Task<IActionResult> createDocumentType([FromBody] DocumentTypeDto documentTypeDto)
        {
            try
            {
                await _ProcedureServices.createDocumentType(documentTypeDto);
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

        [HttpPost("QueryField")]

        public async Task<IActionResult> createQueryField([FromBody] QueryFieldDto queryFieldDto)
        {
            try
            {
                await _ProcedureServices.createQueryField(queryFieldDto);

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
            
            
        [HttpPost("/Add/TypeSocialMedia_ToMunicipality")]
        public async Task<IActionResult> AsignSocialMediaToMunicipality([FromBody] MunicipalitySocialMeditaDto_Response dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Petición sin cuerpo en AsignSocialMediaToMunicipality.");
                return BadRequest("Datos inválidos.");
            }

            var result = await _ProcedureServices.AddMuncipalitySocialMediaToMunicipality(dto);
            if (!result)
            {
                return BadRequest("No se pudo asignar la red social al municipio.");
            }
            return Ok("Red social asignada correctamente.");
        }
        
        
        [HttpPost("Availibity")]

        public async Task<IActionResult> createAvailibity([FromBody] AvailibityDto availibityDto)
        {
            try
            {
                await _ProcedureServices.createAvailibity(availibityDto);

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


    }
}