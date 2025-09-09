using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
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
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = string.Join(" | ", errors)
                });
            }

            try
            {
                var municipality = await _unitOfWork
                    .genericRepository<Municipality>()
                    .GetByIdAsync(id);

                if (municipality == null)
                {
                    return NotFound(new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "Error: municipio no encontrado"
                    });
                }

                var request = new authenticationRequestFintechDto
                {
                    Username = municipality.UserFintech,
                    Password = municipality.PasswordFintech
                };

                var responseAuthentication = await _fintechService.AuthenticateFintechAsyn(request);
                if (responseAuthentication == null || !responseAuthentication.isSuccess)
                {
                    return BadRequest(responseAuthentication);
                }

                var responseTransaction = await _fintechService.transactionFintech(
                    transactionFintech,
                    responseAuthentication.result
                );

                if (responseTransaction == null || !responseTransaction.isSuccess)
                {
                    return BadRequest(responseTransaction);
                }
                return Ok(responseTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error interno: {ex.Message}"
                });
            }
        }

    }

}