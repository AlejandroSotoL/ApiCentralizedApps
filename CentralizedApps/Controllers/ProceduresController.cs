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

        [HttpPost]
        public async Task<IActionResult> createCurseSports([FromBody] AddCourseSportsFacilityDto courseSportsFacilityDto)
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

        [HttpPost("/Add/TypeSocialMedia")]
        public async Task<IActionResult> PostSocialMediaType(SocialMediaTypeDto socialMediaTypeDto)
        {
            var response = await _ProcedureServices.AddSocialMediaType(socialMediaTypeDto);
            return Ok(response);
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
    }
}