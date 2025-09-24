using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CentralizedApps.Controllers.web
{
    public class MuncipalityWebController : Controller
    {
        private readonly IMunicipalityServices _MunicipalityServices;
        public MuncipalityWebController(IMunicipalityServices MunicipalityServices)
        {
            _MunicipalityServices = MunicipalityServices;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _MunicipalityServices.GetAllMunicipalityWithRelations();
                if (response == null || !response.Any())
                {
                    return View(new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "No se encontraron municipios con relaciones."
                    });
                }
                
                return View(response);
            }
            catch (Exception ex)
            {
                return View(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error extraño ${ex.Message}"
                });
            }
        }

    }
}