using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Application.DTOS;
using CentralizedApps.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _Unit;
        public AuthController(IUnitOfWork Unit)
        {
            _Unit = Unit;
        }

        [HttpPost]
        public async Task<ActionResult<ValidationResponseDto>> LoginAsync([FromBody] LoginUserDTO login)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                return BadRequest(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = error ?? "Error de validación"
                });
            }

            var status = await _Unit.AuthRepositoryUnitOfWork.Login(login.Email, login.Password);

            if (!status.BooleanStatus)
                return NotFound(status);

            return Ok(status);
        }
    }
}