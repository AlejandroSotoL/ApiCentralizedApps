using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.EmailDto;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("SendEmail")]
        public async Task<ValidationResponseDto> SendEmail([FromBody] EmailDto emailDto)
        {
            try
            {
                return await _unitOfWork.configurationEmail
                    .EmailConfiguration(emailDto.Subject, emailDto.Body, emailDto.To);
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    SentencesError = "Error sending email: " + e.Message,
                    BooleanStatus = false
                };
            }
        }
    }
}