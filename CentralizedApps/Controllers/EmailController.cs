using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.EmailDto;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    
    public class EmailController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        [HttpGet("SendEmail/ValidationCode")]
        public async Task<ValidationResponseExtraDto> SendEmailValidationCode([FromQuery] string To)
        {
            try
            {
                if (To.IsNullOrEmpty())
                {
                    return new ValidationResponseExtraDto
                    {
                        CodeStatus = 400,
                        SentencesError = "Email address is required.",
                        BooleanStatus = false,
                        ExtraData = null
                    };
                }

                var response = await _unitOfWork.configurationEmail.SendEmailValidationCode(To);
                return response;
            }
            catch (Exception e)
            {
                return new ValidationResponseExtraDto
                {
                    CodeStatus = 500,
                    SentencesError = "Error sending validation code: " + e.Message,
                    BooleanStatus = false,
                    ExtraData = null
                };
            }
        }


        [HttpPost("SendEmail")]
        public async Task<ValidationResponseDto> SendEmail([FromBody] EmailDto emailDto)
        {
            try
            {
                return await _unitOfWork.configurationEmail
                    .SendEmail(emailDto.To, emailDto.Subject, emailDto.Body);
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


        [HttpPost("SendEmail/Panic")]
        public async Task<ValidationResponseDto> SendEmailPanic([FromBody] EmailDtoPanic emailDto)
        {
            try
            {
                if (emailDto == null)
                {
                    return new ValidationResponseDto { BooleanStatus = false, SentencesError = "Datos inválidos" };
                }

                return await _unitOfWork.configurationEmail.SendEmailPanic(emailDto);
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    SentencesError = "Error en endpoint Panic: " + e.Message,
                    BooleanStatus = false
                };
            }
        }

        [HttpPost("SendEmail/Reservations")]
        public async Task<ValidationResponseDto> SendEmailReservations([FromBody] EmailDtoReservations emailDto)
        {
            try
            {
                if (emailDto == null)
                {
                    return new ValidationResponseDto { BooleanStatus = false, SentencesError = "Datos inválidos" };
                }

                return await _unitOfWork.configurationEmail.SendEmailReservation(emailDto);
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    SentencesError = "Error en endpoint Reservations: " + e.Message,
                    BooleanStatus = false
                };
            }
        }

    }
}