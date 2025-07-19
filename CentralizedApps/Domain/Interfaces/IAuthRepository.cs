using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Application.DTOS;

namespace CentralizedApps.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<ValidationResponseDto> Login(string Email , string Password);
    }
}