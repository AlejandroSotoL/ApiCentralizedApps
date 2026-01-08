using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services;
using CentralizedApps.Services.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]

    public class PeopleInvitatedController : ControllerBase
    {
        private readonly IPeopleInvitated _peopleInvitated;

        public PeopleInvitatedController(
            IPeopleInvitated peopleInvitated
        )
        {
            _peopleInvitated = peopleInvitated;
        }

        [HttpPost("Create")]
        public async Task<ValidationResponseDto> AddPeopleInvitatedCon([FromBody] CreatePeopleInvitated createPeopleInvitated)
        {
            try
            {
                var result = await _peopleInvitated.AddPeopleInvitated(createPeopleInvitated);
                return result;
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 500,
                    BooleanStatus = false,
                    SentencesError = $"Tenemos problemas -> {e.Message}"
                };
            }
        }
    }

}