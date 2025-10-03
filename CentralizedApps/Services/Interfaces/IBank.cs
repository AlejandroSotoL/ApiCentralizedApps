using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;

namespace CentralizedApps.Services.Interfaces
{
    public interface IBank
    {
        Task<ValidationResponseDto> CreateBank(CreateBankDto bankAccountDto);
        Task<ValidationResponseDto> updateBank(int id, CreateBankDto bankAccountDto);
    }
}