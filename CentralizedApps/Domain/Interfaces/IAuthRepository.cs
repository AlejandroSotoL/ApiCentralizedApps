using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> Login(string Email , string Password);
    }
}