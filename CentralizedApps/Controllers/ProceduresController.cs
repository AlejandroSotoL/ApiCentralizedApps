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


        [HttpPut("Update/Status/Course/{id}")]
        public async Task<ValidationResponseDto> UpdateStatusCourse(int id, bool status)
        {
            try
            {
                var result = await _ProcedureServices.UpdateStatusCourse(id, status);
                if (!result.BooleanStatus)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = result.BooleanStatus,
                        CodeStatus = result.CodeStatus,
                        SentencesError = "Error al actualizar el estado del curso: " + result.SentencesError
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
                    SentencesError = "Error al actualizar el estado del curso: " + ex.Message
                };
            }
        }

        
        [HttpPut("Update/Status/SportFacilietes/{id}")]
        public async Task<ValidationResponseDto> UpdateStatusSportFacilietes(int id, bool status)
        {
            try
            {
                var result = await _ProcedureServices.UpdateStatusSportFacilietes(id, status);
                if (!result.BooleanStatus)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = result.BooleanStatus,
                        CodeStatus = result.CodeStatus,
                        SentencesError = "Error al actualizar el estado del sport: " + result.SentencesError
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
                    SentencesError = "Error al actualizar el estado del sport: " + ex.Message
                };
            }
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

        [HttpPut("News/{id}")]
        public async Task<ValidationResponseDto> UpdateNews(int id, [FromBody] NewsByMunicipalityDto newsByMunicipalityDto)
        {
            try
            {
                if (newsByMunicipalityDto == null)
                {
                    return new ValidationResponseDto
                    {
                        CodeStatus = 400,
                        BooleanStatus = false,
                        SentencesError = "NewsByMunicipalityDto no puede ser nulo."
                    };
                }

                var response = await _ProcedureServices.updateNews(id, newsByMunicipalityDto);
                return new ValidationResponseDto
                {
                    CodeStatus = response.CodeStatus,
                    BooleanStatus = response.BooleanStatus,
                    SentencesError = response.SentencesError
                };
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 500,
                    BooleanStatus = false,
                    SentencesError = "Error al actualizar la noticia: " + e.Message
                };
            }
            }

    }
}


