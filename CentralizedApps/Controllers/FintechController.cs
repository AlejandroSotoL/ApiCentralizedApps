using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.DtosFintech;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FintechController : ControllerBase
    {
        private readonly IFintechService _fintechService;
        private readonly IUnitOfWork _unitOfWork;

        public FintechController(IFintechService fintechService, IUnitOfWork unitOfWork)
        {
            _fintechService = fintechService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("transactionFintech/{id}")]
        public async Task<IActionResult> transactionFintech([FromBody] TransactionFintech transactionFintech, int id)
        {
            var mucipality = await _unitOfWork.genericRepository<Municipality>().GetByIdAsync(id);
            if (mucipality == null)
            {
                return NotFound(" municipio no encontrado");
            }

            var request = new authenticationRequestFintechDto
            {
                Username = "MercadoLibreDivisas",
                Password = "Gobernacion_9013387663_1Cero12023$/*-"
            };

            var responseauthentication = await _fintechService.AuthenticateFintechAsyn(request);
            if (responseauthentication == null)
            {
                return BadRequest("no se pudo autenticar");
            }
            Console.WriteLine(responseauthentication.result);
            var responseTransaction = await _fintechService.transactionFintech(transactionFintech, responseauthentication.result);

            return Ok(responseTransaction);
        }

    }

}