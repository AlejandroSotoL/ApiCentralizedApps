using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessController : ControllerBase
    {

        private readonly IProcedureServices _ProcedureServices;
        private readonly IBank _BankService;
        public ProcessController(IProcedureServices ProcedureServices, IBank BankService)
        {
            _ProcedureServices = ProcedureServices;
            _BankService = BankService;
        }

        [HttpPost("Create/Shield")]
        public async Task<ValidationResponseDto> CreateShield([FromBody] ShieldMunicipalityDto createShieldDto)
        {
            try
            {
                var response = await _ProcedureServices.createShield(createShieldDto);
                return new ValidationResponseDto
                {
                    CodeStatus = response.CodeStatus,
                    BooleanStatus = response.BooleanStatus,
                    SentencesError = response.SentencesError
                };
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

        [HttpPost("Create/Bank")]
        public async Task<ValidationResponseDto> CreateBank([FromBody] CreateBankDto createBankDto) {
            try
            {                
                var response = await _BankService.CreateBank(createBankDto);
                return new ValidationResponseDto
                {
                    CodeStatus = response.CodeStatus,
                    BooleanStatus = response.BooleanStatus,
                    SentencesError = response.SentencesError
                };
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

        [HttpPost("Create/SocialMediaType")]
        public async Task<IActionResult> createSocialMediaType([FromBody] CreateSocialMediaTypeDto createSocialMediaTypeDto)
        {
            try
            {
                await _ProcedureServices.createSocialMediaType(createSocialMediaTypeDto);

                return Ok(new ValidationResponseDto
                {
                    CodeStatus = 200,
                    BooleanStatus = true,
                    SentencesError = ""
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = ex.Message
                });
            }
        }

        [HttpPost("Create/NewNotice")]
        public async Task<ValidationResponseDto> CreateNewNotice( NewsByMunicipalityDto newsByMunicipalityDto)
        {
            try
            {
                if (newsByMunicipalityDto == null)
                {
                    return new ValidationResponseDto
                    {
                        CodeStatus = 400,
                        BooleanStatus = false,
                        SentencesError = "NewsByMunicipalityDto no puede ser nulo."
                    };
                    
                }
                var response = await _ProcedureServices.createNewNotice(newsByMunicipalityDto);
                return new ValidationResponseDto
                {
                    CodeStatus = response.CodeStatus,
                    BooleanStatus = response.BooleanStatus,
                    SentencesError = response.SentencesError
                };
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 500,
                    BooleanStatus = false,
                    SentencesError = "Tenemos problemas técnicos, por favor intente más tarde." + e.Message
                };
            }
        }


        [HttpPost("Create/NewTheme")]
        public async Task<ValidationResponseDto> CreateNewTheme([FromBody] ThemeDto themeDto)
        {
            if (themeDto == null)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = "ThemeDto no puede ser nulo."
                };
            }

            var response = await _ProcedureServices.createNewTheme(themeDto);
            return new ValidationResponseDto
            {
                CodeStatus = response.CodeStatus,
                BooleanStatus = response.BooleanStatus,
                SentencesError = response.SentencesError
            };
        }

        //Make a new type of procedure for municipalities.
        [HttpPost("Create/NewTypeProcedure")]
        public async Task<ValidationResponseDto> createNewTypeProcedure(CreateProcedureDto typeProcedureDto)
        {
            try
            {
                var response = await _ProcedureServices.createNewTypeProcedure(typeProcedureDto);
                return new ValidationResponseDto
                {
                    CodeStatus = response.CodeStatus,
                    BooleanStatus = response.BooleanStatus,
                    SentencesError = response.SentencesError
                };
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = ex.Message
                };
            }
        }


        [HttpPost("Create/QueryField")]
        public async Task<IActionResult> createQueryField([FromBody] QueryFieldDto queryFieldDto)
        {
            try
            {
                await _ProcedureServices.createQueryField(queryFieldDto);
                return Ok(new ValidationResponseDto
                {
                    CodeStatus = 200,
                    BooleanStatus = true,
                    SentencesError = ""
                });


            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = ex.Message
                });
            }
        }


        [HttpPost("Create/Availibity")]
        public async Task<IActionResult> createAvailibity([FromBody] CreateAvailibityDto availibityDto)
        {
            try
            {
                await _ProcedureServices.createAvailibity(availibityDto);

                return Ok(new ValidationResponseDto
                {
                    CodeStatus = 200,
                    BooleanStatus = true,
                    SentencesError = ""
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = ex.Message
                });
            }
        }


    }
}