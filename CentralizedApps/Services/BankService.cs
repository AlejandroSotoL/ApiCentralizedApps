using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CentralizedApps.Services
{
    public class BankService : IBank
    {
        private readonly IUnitOfWork _unitOfWork;
        public BankService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ValidationResponseDto> CreateBank(CreateBankDto bankAccountDto)
        {
            try
            {
                if (bankAccountDto.NameBank.IsNullOrEmpty())
                {
                    return new ValidationResponseDto
                    {
                        CodeStatus = 400,
                        BooleanStatus = false,
                        SentencesError = "Bank name cannot be empty."
                    };
                }

                var response = await _unitOfWork.genericRepository<Bank>()
                    .FindAsync_Predicate(x => x.NameBank == bankAccountDto.NameBank);
                    
                if (response != null)
                {
                    return new ValidationResponseDto
                    {
                        CodeStatus = 400,
                        BooleanStatus = false,
                        SentencesError = "Ese Banco ya existe."
                    };
                }

                var strucutr = new Bank
                {
                    NameBank = bankAccountDto.NameBank,
                };

                await _unitOfWork.genericRepository<Bank>().AddAsync(strucutr);
                var rows = await _unitOfWork.SaveChangesAsync();
                if (rows > 0) {
                    return new ValidationResponseDto
                    {
                        CodeStatus = 200,
                        BooleanStatus = true,
                        SentencesError = "Banco creado correctamente. Filas: " + rows
                    };
                } else {
                    return new ValidationResponseDto
                    {
                        CodeStatus = 400,
                        BooleanStatus = false,
                        SentencesError = "No se pudo crear el banco."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = $"Error: {ex.Message}"
                };

            }
        }
    }
}