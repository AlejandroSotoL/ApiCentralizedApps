using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentHistoryController : ControllerBase
    {

        private readonly IPaymentHistoryService _paymentHistoryService;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentHistoryController(IPaymentHistoryService paymentHistoryService, IUnitOfWork unitOfWork)
        {
            _paymentHistoryService = paymentHistoryService;
            _unitOfWork = unitOfWork;
        }



        [HttpGet("User/{id}")]
        public async Task<IActionResult> getAllPaymentHistory(int id)
        {
            if (id <= 0)
            {
                return  BadRequest(new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = "el id no debe ser null y debe se mayor a cero"
                });
            }
            var response = await _paymentHistoryService.getAllPaymentHistoryByIdAsync(id);
            if (response == null || !response.Any())
            {
                return NotFound(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = $"Notfound"
                });
            }
            else
            {

                return Ok(response);
            }

        }

        [HttpPost]

        public async Task<IActionResult> createPaymentHistory([FromBody] PaymentHistoryDto createPaymentHistoryDto)
        {
            try
            {

                if (createPaymentHistoryDto == null)
                {
                    return BadRequest(new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = "el objeto no puede ser null"
                });
                }

                await _paymentHistoryService.createPaymentHistory(createPaymentHistoryDto);
                return Ok(new ValidationResponseDto
                {
                    CodeStatus = 200,
                    BooleanStatus = true,
                    SentencesError = ""
                });
            }
            catch (Exception ex)
            {
                return  BadRequest(new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = ex.Message
                });
            }
            
            
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> updatePaymentHistory(int id, int idStatusType)
        {
            try
            {
                
                if (idStatusType <= 0)
                {
                    return BadRequest(
                    new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = "el id no puede ser null y debe ser mayor a cero"
                    });
                }

                var result = await _paymentHistoryService.UpdatePaymentHistory(id, idStatusType);
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
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = ex.Message
                });
            }
        }
    
    }
}