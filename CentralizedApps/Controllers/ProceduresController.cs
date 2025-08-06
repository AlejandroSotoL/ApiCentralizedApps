using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MunicipalityServices> _logger;
        public ProceduresController(ILogger<MunicipalityServices> logger, IProcedureServices ProcedureServices, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _ProcedureServices = ProcedureServices;
        }

        [HttpPost("NewTypeProcedure")]
        public async Task<ValidationResponseDto> createNewTypeProcedure(CreateProcedureDto typeProcedureDto)
        {
            try
            {
                var response = await _ProcedureServices.createNewTypeProcedure(typeProcedureDto);
                return new ValidationResponseDto
                {
                    CodeStatus = response.CodeStatus,
                    BooleanStatus = response.BooleanStatus,
                    SentencesError = response.SentencesError
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
        }
        [HttpPost("SocialMediaType")]

        public async Task<IActionResult> createSocialMediaType([FromBody] CreateSocialMediaTypeDto createSocialMediaTypeDto)
        {
            try
            {
                await _ProcedureServices.createSocialMediaType(createSocialMediaTypeDto);

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

        [HttpPost("AsingProccessToMunicipality")]
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

        [HttpPost("TypeSocialMedia_ToMunicipality")]
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
        public async Task<IActionResult> createAvailibity([FromBody] CreateAvailibityDto availibityDto)
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
        [HttpPost("Course")]
        public async Task<IActionResult> createCourse([FromBody] CreateCourseDto createCourseDto)
        {
            try
            {
                await _ProcedureServices.createCourse(createCourseDto);

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
        [HttpPost("SportsFacility")]
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


        [HttpPut("DocumentType/{id}")]
        public async Task<IActionResult> updateDocumentType(int id, [FromBody] DocumentTypeDto updateDocumentTypeDto)
        {

            try
            {
                if (updateDocumentTypeDto == null)
                {
                    return BadRequest(
                    new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "el objeto no puede ser null"
                    });
                }

                var result = await _ProcedureServices.updateDocumentType(id, updateDocumentTypeDto);

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
                return Ok(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Error: " + ex.Message
                });

            }
        }



        [HttpPost("NewTheme")]
        public async Task<IActionResult> createNewTheme([FromBody] ThemeDto themeDto)
        {
            _logger.LogInformation("Creating new theme with data: {@ThemeDto}", themeDto);
            if (themeDto == null)
            {
                _logger.LogWarning("Received null themeDto in createNewTheme.");
                return BadRequest("Theme data is required.");
            }

            var response = await _ProcedureServices.createNewTheme(themeDto);
            if (response == null)
            {
                _logger.LogError("Failed to create new theme.");
                return BadRequest("Failed to create new theme.");
            }
            return Ok(response);
        }

        // PUT
        [HttpPut("Update/NewTheme/{id}")]
        public async Task<ValidationResponseDto> UpdateTheme(int id, ThemeDto procedureDto)
        {
            try
            {
                var result = await _ProcedureServices.UpdateTheme(id, procedureDto);
                if (!result.BooleanStatus)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = result.BooleanStatus,
                        CodeStatus = result.CodeStatus,
                        SentencesError = "Error al actualizar el tema: " + result.SentencesError
                    };
                }

                return new ValidationResponseDto
                {
                    BooleanStatus = result.BooleanStatus,
                    CodeStatus = result.CodeStatus,
                    SentencesError = "Actualizado " + result.SentencesError
                };

            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = "Error al actualizar el tema: " + ex.Message
                };
            }
        }


        [HttpPut("QueryField/{id}")]
        public async Task<IActionResult> updateQueryField(int id, [FromBody] QueryFieldDto updatequeryFieldDto)
        {

            try
            {
                if (updatequeryFieldDto == null)
                {
                    return BadRequest(
                    new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "el objeto no puede ser null"
                    });
                }

                var result = await _ProcedureServices.updateQueryField(id, updatequeryFieldDto);

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
                return Ok(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Error: " + ex.Message
                });
            }
        }


        [HttpPut("Availibity/{id}")]
        public async Task<IActionResult> updateAvailibity(int id, [FromBody] CreateAvailibityDto updateAvailibityDto)
        {

            try
            {
                if (updateAvailibityDto == null)
                {
                    return BadRequest(
                    new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "el objeto no puede ser null"
                    });
                }

                var result = await _ProcedureServices.updateAvailibity(id, updateAvailibityDto);

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
                return Ok(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Error: " + ex.Message
                });
            }
        }

        [HttpPut("Course/{id}")]
        public async Task<IActionResult> updateCourse(int id, [FromBody] CreateCourseDto updateCourseDto)
        {

            try
            {
                if (updateCourseDto == null)
                {
                    return BadRequest(
                    new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "el objeto no puede ser null"
                    });
                }

                var result = await _ProcedureServices.updateCourse(id, updateCourseDto);

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
                return Ok(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = "Error al actualizar el tema: " + ex.Message
                });
            }
        }




        [HttpPut("SportsFacility/{id}")]
        public async Task<IActionResult> updateSportsFacility(int id, [FromBody] CreateSportsFacilityDto updateSportsFacilityDto)
        {

            try
            {
                if (updateSportsFacilityDto == null)
                {
                    return BadRequest(
                    new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = "el objeto no puede ser null"
                    });
                }

                var result = await _ProcedureServices.updateSportsFacility(id, updateSportsFacilityDto);

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

        [HttpPut("SocialMediaType/{id}")]
        public async Task<IActionResult> updateSocialMediaType(int id, [FromBody] CreateSocialMediaTypeDto updateSocialMediaTypeDto)
        {

            try
            {
                if (updateSocialMediaTypeDto == null)
                {
                    return BadRequest(
                    new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = "el objeto no puede ser null"
                    });
                }

                var result = await _ProcedureServices.updateSocialMediaType(id, updateSocialMediaTypeDto);

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


        [HttpPut("MunicipalitySocialMedium/{id}")]
        public async Task<IActionResult> updateMunicipalitySocialMedium(int id, [FromBody] CreateMunicipalitySocialMediumDto updateMunicipalitySocialMediumDto)
        {

            try
            {
                if (updateMunicipalitySocialMediumDto == null)
                {
                    return BadRequest(
                    new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = "el objeto no puede ser null"
                    });
                }

                var result = await _ProcedureServices.updateMunicipalitySocialMedium(id, updateMunicipalitySocialMediumDto);

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


        [HttpPut("UpdateMuncipality/{Id}")]
        public async Task<ValidationResponseDto> UpdateMunicipality(int Id , CompleteMunicipalityDto MunicipalityDTO)
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

            var result = await _ProcedureServices.UpdateMunicipality(Id , MunicipalityDTO);
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
    }
}


