using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;

namespace CentralizedApps.Controllers.web
{
    public class MunicipalityController : Controller
    {
        private readonly IMunicipalityServices _MunicipalityServices;
        private readonly IProcedureServices _ProcedureServices;
        public MunicipalityController(IMunicipalityServices MunicipalityServices, IProcedureServices ProcedureServices)
        {
            _MunicipalityServices = MunicipalityServices;
            _ProcedureServices = ProcedureServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? filter)
        {
            try
            {
                var response = await _MunicipalityServices.GetAllMunicipalityWithRelations();
                if (!string.IsNullOrEmpty(filter))
                {
                    filter.ToLower();

                    response = response.Where(municipio =>
                    (municipio.Name != null && municipio.Name.ToLower().Contains(filter)) ||
                    (municipio.Department?.Name != null && municipio.Department.Name.ToLower().Contains(filter)) ||
                    (municipio.Bank?.NameBank != null && municipio.Bank.NameBank.ToLower().Contains(filter))
                    ).ToList();
                }
                if (response == null || !response.Any())
                {
                    return View(response);
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




        [HttpPost]
        public async Task<IActionResult> UpdateStatusMunicipality(int id, bool isActive)
        {
            Console.WriteLine($"LLEGA al método con id={id}, isActive={isActive}");
            try
            {
                var response = await _ProcedureServices.UpdateStatusMunicipality(id, isActive);
                Console.WriteLine("respuesta " + response);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> updateMunicipality(int id)
        {
            var response = await _MunicipalityServices.JustGetMunicipalityWithRelations(id);
            return Json(response);
        }


    }
}