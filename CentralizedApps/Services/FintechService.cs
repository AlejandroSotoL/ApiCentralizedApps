using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.HttpClients;
using CentralizedApps.Models.Dtos.DtosFintech;
using CentralizedApps.Services.Interfaces;

namespace CentralizedApps.Services
{
    public class FintechService : IFintechService
    {

        private readonly FintechApiClient _fintechApiClient;
        public FintechService(FintechApiClient fintechApiClient)
        {
            _fintechApiClient = fintechApiClient;
        }
        public async Task<authenticationResponseFintechDto> AuthenticateFintechAsyn(authenticationRequestFintechDto requestFintech)
        {
            return await _fintechApiClient.AutenticationFintech(requestFintech);
        }

        public async Task<TransactionResponse> transactionFintech(TransactionFintech transactionFintech, string token)
        {
            return await _fintechApiClient.TransactionFintech(transactionFintech, token);
        }
    }
}