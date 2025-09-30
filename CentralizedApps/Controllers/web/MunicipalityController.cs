using AutoMapper;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers.web
{
    public class MunicipalityController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMunicipalityServices _municipalityServices;
        private readonly IProcedureServices _procedureServices;
        private readonly IUnitOfWork _unitOfWork;

        public MunicipalityController(
            IMunicipalityServices municipalityServices,
            IProcedureServices procedureServices,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _municipalityServices = municipalityServices;
            _procedureServices = procedureServices;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _municipalityServices.GetAllMunicipalityWithRelations();
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> FormMunicipality(int id)
        {
            try
            {
                var municipality = await _municipalityServices.JustGetMunicipalityWithRelationsWeb(id);
                return View(municipality ?? new MunicipalityDto());
            }
            catch
            {
                return View(new MunicipalityDto());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentDto department)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("FormMunicipality");

            try
            {
                var mapperDepartment = _mapper.Map<Department>(department);
                await _unitOfWork.genericRepository<Department>().AddAsync(mapperDepartment);
                await _unitOfWork.SaveChangesAsync();

                TempData["Mensaje"] = "Departamento creado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["Error"] = $"Error al crear departamento: {e.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTheme(MunicipalityDto model)
        {
            try
            {
                var themeDto = model.municipality?.Theme;
                if (themeDto == null)
                {
                    TempData["Error"] = "No se envió ningún tema.";
                    return RedirectToAction("FormMunicipality");
                }

                var mapperTheme = _mapper.Map<Theme>(themeDto);
                await _unitOfWork.genericRepository<Theme>().AddAsync(mapperTheme);
                await _unitOfWork.SaveChangesAsync();

                TempData["Mensaje"] = "Tema creado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al crear tema: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBank(CreateBankDto bank)
        {
            try
            {
                var mapperBank = _mapper.Map<Bank>(bank);
                await _unitOfWork.genericRepository<Bank>().AddAsync(mapperBank);
                await _unitOfWork.SaveChangesAsync();

                TempData["Mensaje"] = "Banco creado correctamente.";
                return View("Index");
            }
            catch (Exception e)
            {
                TempData["Error"] = $"Error al crear banco: {e.Message}";
                return View("Index");
            }
        }

        
    }
}
