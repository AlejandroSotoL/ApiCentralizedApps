using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;

using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Services;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CentralizedApps.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MunicipalityController : ControllerBase
    {
        private readonly IMunicipalityServices _MunicipalityServices;
        private readonly ILogger<MunicipalityServices> _logger;
        public MunicipalityController(ILogger<MunicipalityServices> logger, IMunicipalityServices MunicipalityServices)
        {
            _MunicipalityServices = MunicipalityServices;
            _logger = logger;
        }

        [HttpPost("New")]
        public async Task<IActionResult> AddMunicipalityAsync([FromBody] CompleteMunicipalityDto dto)
        {
            try
            {
                var result = await _MunicipalityServices.AddMunicipalityAsync(dto);
                if (!result.BooleanStatus)
                {
                    return BadRequest(new
                    {
                        success = result.BooleanStatus,
                        code = result.CodeStatus,
                        error = result.SentencesError
                    });
                }

                return Ok(new
                {
                    success = result.BooleanStatus,
                    code = result.CodeStatus,
                    message = result.SentencesError
                });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error extraño ${ex.Message}"
                });
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllMunicipalityWithRelations()
        {
            try
            {
                var response = await _MunicipalityServices.GetAllMunicipalityWithRelations();
                if (response == null || !response.Any())
                {
                    return BadRequest(new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "No se encontraron municipios con relaciones."
                    });
                }
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error extraño ${ex.Message}"
                });
            }
        }


        [HttpGet("ByDepartamet_{Id}")]
        public async Task<IActionResult> JustMunicipalitys(int Id)
        {
            try
            {
                var relations = await _MunicipalityServices.justMunicipalitysDtos(Id);
                if (relations == null || !relations.Any())
                {
                    return BadRequest(new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "No se encontraron municipios con relaciones."
                    });
                }
                
                return Ok(relations);
            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error extraño ${ex.Message}"
                });
            }
        }

        [HttpGet("GetInfoBy{Id}")]
        public async Task<IActionResult> GetJust(int Id)
        {
            var response = await _MunicipalityServices.JustGetMunicipalityWithRelations(Id);
            if (response == null)
            {
                return BadRequest(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = $"No se encontro el municipio con ID {Id}."
                });
            }
            return Ok(response);
        }
    }
}