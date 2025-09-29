using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CentralizedApps.Controllers.web
{
    public class MunicipalityController : Controller
    {
        private readonly IMunicipalityServices _MunicipalityServices;
        private readonly IProcedureServices _ProcedureServices;
        private readonly IUnitOfWork _unitOfWork;
        public MunicipalityController(IMunicipalityServices MunicipalityServices, IProcedureServices ProcedureServices, IUnitOfWork unitOfWork)
        {
            _MunicipalityServices = MunicipalityServices;
            _ProcedureServices = ProcedureServices;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? filter)
        {
            try
            {
                var response = await _MunicipalityServices.GetAllMunicipalityWithRelationsWeb(filter);
                if (response == null || !response.Any())
                {
                    return View(new List<GetMunicipalitysDto>());
                }
                return View(response);
            }
            catch (Exception ex)
            {
                return View(new List<GetMunicipalitysDto>());
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
        public async Task<IActionResult> FormMunicipality(int id)
        {

            try
            {
                var municipality = await _MunicipalityServices.JustGetMunicipalityWithRelationsWeb(id);

                if (municipality == null)
                {
                    return View(new MunicipalityDto());
                }
                return View(municipality);
            }
            catch (Exception ex)
            {
                return View(new MunicipalityDto());
            }
        }
    }
}