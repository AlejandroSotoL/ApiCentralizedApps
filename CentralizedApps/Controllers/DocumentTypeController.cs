using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IProcedureServices _ProcedureServices;
        public DocumentTypeController(IProcedureServices ProcedureServices)
        {
            _ProcedureServices = ProcedureServices;
        }

        [HttpGet("GetDocumentTypes")]
        public async Task<List<DocumentType>> GetDocumentTypes()
        {
            return await _ProcedureServices.GetDocumentTypes();
        }

                [HttpPost("DocumentType")]
        public async Task<IActionResult> createDocumentType([FromBody] DocumentTypeDto documentTypeDto)
        {
            try
            {
                await _ProcedureServices.createDocumentType(documentTypeDto);
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

        
        [HttpPut("DocumentType/{id}")]
        public async Task<IActionResult> updateDocumentType(int id, [FromBody] DocumentTypeDto updateDocumentTypeDto)
        {

            try
            {
                if (updateDocumentTypeDto == null)
                {
                    return BadRequest(
                    new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "el objeto no puede ser null"
                    });
                }

                var result = await _ProcedureServices.updateDocumentType(id, updateDocumentTypeDto);

                if (!result.BooleanStatus)
                {
                    return BadRequest(new ValidationResponseDto
                    {
                        BooleanStatus = result.BooleanStatus,
                        CodeStatus = result.CodeStatus,
                        SentencesError = "Error: " + result.SentencesError
                    });
                }
                else
                {
                    return Ok(new ValidationResponseDto
                    {
                        BooleanStatus = result.BooleanStatus,
                        CodeStatus = result.CodeStatus,
                        SentencesError = result.SentencesError
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Error: " + ex.Message
                });

            }
        }

    }
}