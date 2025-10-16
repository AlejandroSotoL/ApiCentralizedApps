using System.Threading.Tasks;
using AutoMapper;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.RemidersDto;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using CentralizedApps.Services.ServicesWeb.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers.web
{
    public class GeneralProceduresController : Controller
    {
        private readonly IMunicipalityServices _MunicipalityServices;
        private readonly IGeneralProcedures _GeneralProcedure;
        private readonly IProcedureServices _ProcedureServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GeneralProceduresController(IMunicipalityServices MunicipalityServices, IProcedureServices ProcedureServices, IUnitOfWork unitOfWork, IMapper mapper, IGeneralProcedures GeneralProcedure)
        {
            _MunicipalityServices = MunicipalityServices;
            _ProcedureServices = ProcedureServices;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _GeneralProcedure = GeneralProcedure;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var structs = new CreateMunicipalityProcedures_Web
                {
                    Municipalities = await _unitOfWork.genericRepository<Municipality>().GetAllAsync(),
                    Procedures = await _unitOfWork.genericRepository<Procedure>().GetAllAsync(),
                };
                return View(structs);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> BringResultById(int id)
        {
            var response = await _GeneralProcedure.MunicipalityProcedures(id);

            var model = new CreateMunicipalityProcedures_Web
            {
                Municipalities = await _unitOfWork.genericRepository<Municipality>().GetAllAsync(),
                Procedures = await _unitOfWork.genericRepository<Procedure>().GetAllAsync(),
                ProceduresWithRelations = response ?? new List<MunicipalityProcedureDto_Reminders>()
            };

            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMunicipalityProcedure(int MunicipalityId, MunicipalityProcedureDto_Reminders dto)
        {
            try
            {
                var response = await _unitOfWork.genericRepository<MunicipalityProcedure>()
                                                 .FindAsync_Predicate(x => x.Id == dto.Id);

                if (response == null || response.MunicipalityId != MunicipalityId)
                {
                    TempData["Error"] = $"Problema al identificar el proceso o la alcaldía. -> llego {MunicipalityId}  - es {response?.Id}";
                    return RedirectToAction("Index");
                }

                response.ProceduresId = dto.Procedures?.Id ?? response.ProceduresId;
                response.IntegrationType = dto.IntegrationType;
                response.IsActive = dto.IsActive ?? false;

                var rows = await _unitOfWork.SaveChangesAsync();
                if (rows > 0)
                    TempData["Success"] = "El proceso se ha editado correctamente.";
                else
                    TempData["Error"] = "No se lograron guardar los cambios.";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = "Ocurrió un error al guardar los cambios.";
                return RedirectToAction("Index");
            }
        }


    }
}
