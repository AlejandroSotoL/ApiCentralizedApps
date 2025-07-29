using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
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

        [HttpPost("/Add/TypeSocialMedia")]
        public async Task<IActionResult> PostSocialMediaType(SocialMediaTypeDto socialMediaTypeDto)
        {
            return Ok();
        }


        [HttpPost("/Add/TypeSocialMedia_ToMunicipality")]
        public async Task<IActionResult> AsignSocialMediaToMunicipality()
        {
            return Ok();
        }
    }
}