
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [AllowAnonymous]
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
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = error ?? "Error de validación"
                };
            }
            var status = await _Unit.AuthRepositoryUnitOfWork.Login(login.Email, login.Password);
            if (!status.BooleanStatus)
            {
                status.CodeStatus = 404;
                status.SentencesError = "Alguno de los datos ingresados es incorrecto";
                return status;
            }
            return status;
        }
    }
}