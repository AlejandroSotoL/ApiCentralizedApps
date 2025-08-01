using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public DocumentTypeController( IProcedureServices ProcedureServices )
        {
            _ProcedureServices = ProcedureServices;
        }

        [HttpGet("GetDocumentTypes")]
        public async Task<List<DocumentType>> GetDocumentTypes()
        {
            return await _ProcedureServices.GetDocumentTypes();
        }
    }
}