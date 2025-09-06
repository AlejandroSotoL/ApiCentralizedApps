using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.DtosFintech;

namespace CentralizedApps.Services.Interfaces
{
    public interface IFintechService
    {
        Task<authenticationResponseFintechDto> AuthenticateFintechAsyn(authenticationRequestFintechDto requestFintech);
        Task<TransactionResponse> transactionFintech(TransactionFintech transactionFintech, string token);
    }
}