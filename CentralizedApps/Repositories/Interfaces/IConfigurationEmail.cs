using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;

namespace CentralizedApps.Repositories.Interfaces
{
    public interface IConfigurationEmail
    {
        Task<ValidationResponseDto> EmailConfiguration(string Subject, string Body, string To);

    }
}