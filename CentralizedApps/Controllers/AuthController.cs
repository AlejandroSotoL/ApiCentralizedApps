using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> LoginAsync(string Email, string Password)
        {
            var status = await _Unit.AuthRepositoryUnitOfWork.Login(Email , Password);
            return Ok(status);
        }
    }
}