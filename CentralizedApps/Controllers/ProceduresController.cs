using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProceduresController : ControllerBase
    {
        private readonly IProcedureServices _ProcedureServices;
        public ProceduresController(IProcedureServices ProcedureServices)
        {
            _ProcedureServices = ProcedureServices;
        }

        [HttpPost("CurseSports")]


        [HttpGet]
        public async Task<IActionResult> GetAllCurseSportsMunicipality()
        {
            return Ok("");
        }


        [HttpPost]
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


        [HttpPost("Procedures")]

        public async Task<IActionResult> createProcedures([FromBody] ProcedureDto procedureDto)
        {
            try
            {
                await _ProcedureServices.createProcedures(procedureDto);

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


        [HttpPost("/Add/TypeSocialMedia_ToMunicipality")]
        public async Task<IActionResult> AsignSocialMediaToMunicipality()
        {
            return Ok();

        }
    }
}