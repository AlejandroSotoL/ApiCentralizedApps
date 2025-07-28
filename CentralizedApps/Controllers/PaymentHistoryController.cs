using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                


            await _paymentHistoryService.createPaymentHistory(createPaymentHistoryDto);
            await _unitOfWork.SaveChangesAsync();
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

        public async Task<IActionResult> updatePaymentHistory(int id, [FromBody] PaymentHistoryDto PaymentHistoryDto)
        {
            try
            {
                var responsepaymentHistory = await _unitOfWork.paymentHistoryRepository.GetPaymentHistoryByIdAsync(paymentHistory => paymentHistory.Id == id);

                if (responsepaymentHistory == null)
                {
                    return NotFound(new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = $"Notfound"
                    });
                }

                _paymentHistoryService.UpdatePaymentHistory(responsepaymentHistory, PaymentHistoryDto);
                await _unitOfWork.SaveChangesAsync();

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
    
    }
}