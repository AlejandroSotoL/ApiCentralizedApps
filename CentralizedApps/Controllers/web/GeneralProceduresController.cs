using AutoMapper;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.RemidersDto;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using CentralizedApps.Services.ServicesWeb.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers.web
{
    [Authorize(Roles = "Administrador")]
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
                    Municipalities = await _unitOfWork.genericRepository<Municipality>()
                                            .GetAllWithIncludesAsync(x => x.IdShieldNavigation, x => x.Department),
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


        [HttpGet]
        public async Task<IActionResult> GetProceduresByMunicipality(int id)
        {
            try
            {
                var response = await _GeneralProcedure.MunicipalityProcedures(id);
                return Ok(response ?? new List<MunicipalityProcedureDto_Reminders>());
            }
            catch (Exception ex)
            {
                return BadRequest("Error al cargar los procedimientos.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMunicipalityProcedure([FromBody] MunicipalityProcedureAddDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { success = false, message = "Datos inválidos." });
            }

            try
            {
                // Check if procedure already exists for this municipality
                var existing = await _unitOfWork.genericRepository<MunicipalityProcedure>()
                    .FindAsync_Predicate(x => x.MunicipalityId == dto.MunicipalityId && x.ProceduresId == dto.ProceduresId);

                if (existing != null)
                {
                    return BadRequest(new { success = false, message = "Este proceso ya está asignado a la alcaldía seleccionada." });
                }

                var result = await _ProcedureServices.AsingProccessToMunicipality(dto);
                
                if (result.BooleanStatus)
                {
                     return Ok(new { success = true, message = "Proceso agregado correctamente." });
                }
                else
                {
                     return BadRequest(new { success = false, message = result.SentencesError });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error interno del servidor: " + ex.Message });
            }
        }
    }
}
