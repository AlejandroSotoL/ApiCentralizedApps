using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.RemidersDto;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RemindersController : ControllerBase
    {
        private readonly IRemidersService _RemidersService;
        public RemindersController(IRemidersService RemidersService)
        {
            _RemidersService = RemidersService;
        }

        [HttpPost("Create/Reminders")]
        public async Task<ValidationResponseDto> CreateReminders([FromBody] CreateReminderDto createReminderDto)
        {
            try
            {
                var response = await _RemidersService.createReminders(createReminderDto);
                return new ValidationResponseDto
                {
                    CodeStatus = response.CodeStatus,
                    BooleanStatus = response.BooleanStatus,
                    SentencesError = response.SentencesError
                };
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = $"Error: {ex.Message}"
                };
            }
        }

        [HttpGet("Get/Reminders")]
        public async Task<ActionResult<List<ResponseReminderDto>>> GetReminders()
        {
            try
            {
                var response = await _RemidersService.GetReminders();
                return response;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("Get/Reminders/ByUser/{userId}")]
        public async Task<ActionResult<List<ResponseReminderDto>>> GetReminders(int userId)
        {
            try
            { 
                var response = await _RemidersService.GetRemindersByUserId(userId);

                return response;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}