using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.RemidersDto;

namespace CentralizedApps.Services.Interfaces
{
    public interface IRemidersService
    {
        Task<ResponseReminderDto> createReminders(CreateReminderDto createReminderDto);
        Task<List<ResponseReminderDto>> GetReminders();
        Task<List<ResponseReminderDto>> GetRemindersByUserId(int userId);
    }
}