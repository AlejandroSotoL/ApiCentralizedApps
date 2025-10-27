using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.RemidersDto;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    
    public class RemindersController : ControllerBase
    {
        private readonly IRemidersService _RemidersService;
        public RemindersController(IRemidersService RemidersService)
        {
            _RemidersService = RemidersService;
        }


        [HttpPost("Create/Reminders")]
        public async Task<IActionResult> CreateReminders([FromBody] CreateReminderDto createReminderDto)
        {
            try
            {
                var response = await _RemidersService.createReminders(createReminderDto);

                return CreatedAtAction(
                            nameof(GetReminders), // Nombre del método GET para este recurso
                            new { id = createReminderDto.ReminderName }, // Parámetros de ruta para ese método
                            response // El objeto creado (ResponseReminderDto)
                        );
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Error interno al crear el recordatorio: {ex.Message}");
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