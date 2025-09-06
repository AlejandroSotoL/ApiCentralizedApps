using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.DtosFintech;

namespace CentralizedApps.HttpClients
{
    public class FintechApiClient
    {
        private readonly HttpClient _httpClient;

        public FintechApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<authenticationResponseFintechDto> AutenticationFintech(authenticationRequestFintechDto requestFintech)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Login/Auth", requestFintech);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<authenticationResponseFintechDto>();
        }

        public async Task<TransactionResponse> TransactionFintech(TransactionFintech transaction, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("api/Transaction/InsertTransaction", transaction);

            if (!response.IsSuccessStatusCode) // si falla
            {
                // puedes loguear el error o devolver null
                var errorMsg = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error en transacción: {errorMsg}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TransactionResponse>();
        }


    }
}