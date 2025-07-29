using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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