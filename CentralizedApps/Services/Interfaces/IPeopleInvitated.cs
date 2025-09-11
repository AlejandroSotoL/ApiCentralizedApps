using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;

namespace CentralizedApps.Services.Interfaces
{
    public interface IPeopleInvitated
    {
        Task<ValidationResponseDto> AddPeopleInvitated(CreatePeopleInvitated createPeopleInvitated);
    }
}