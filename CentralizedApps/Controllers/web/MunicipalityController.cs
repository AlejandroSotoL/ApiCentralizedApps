using CentralizedApps.Models;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using CentralizedApps.Services.ServicesWeb.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CentralizedApps.Controllers.web
{
    public class MunicipalityController : Controller
    {
        private readonly IMunicipalityServices _MunicipalityServices;
        private readonly IProcedureServices _ProcedureServices;
        private readonly IDepartmentService _departmentService;
        private readonly IGeneralMunicipality _GeneralMunicipality;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWeb _web;
        public MunicipalityController(IMunicipalityServices MunicipalityServices, IProcedureServices ProcedureServices, IUnitOfWork unitOfWork, IDepartmentService departmentService, IWeb web, IGeneralMunicipality GeneralMunicipality)
        {
            _MunicipalityServices = MunicipalityServices;
            _ProcedureServices = ProcedureServices;
            _unitOfWork = unitOfWork;
            _departmentService = departmentService;
            _web = web;
            _GeneralMunicipality = GeneralMunicipality;
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
            catch (Exception e)
            {
                return View(new List<GetMunicipalitysDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> MunicipalitySocialMediaIndex(int? id)
        {
            var response = await _web.MunicipalitiesAndSocialMediaType(id);
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> FormMunicipalityUpdate(int id)
        {
            try
            {
                var structur = new ToAlcaldiaWeb
                {
                    Alcaldia = id > 0
                        ? await _unitOfWork.genericRepository<Municipality>()
                            .GetOneWithNestedIncludesAsync(
                                query => query
                                    .Include(r => r.IdShieldNavigation)!
                                    .Include(r => r.Department)!
                                    .Include(r => r.Theme),
                                m => m.Id == id
                            ) ?? new Municipality()
                        : new Municipality(),

                    Bancos = await _unitOfWork.genericRepository<Bank>().GetAllAsync() ?? new List<Bank>(),
                    Departamentos = await _unitOfWork.genericRepository<Department>().GetAllAsync() ?? new List<Department>(),
                    Escudos = await _unitOfWork.genericRepository<ShieldMunicipality>().GetAllAsync() ?? new List<ShieldMunicipality>(),
                    Themas = await _unitOfWork.genericRepository<Theme>().GetAllAsync() ?? new List<Theme>(),
                };

                return View("FormMunicipality", structur);
            }
            catch
            {
                return View("FormMunicipality", new ToAlcaldiaWeb
                {
                    Alcaldia = new Municipality(),
                    Bancos = new List<Bank>(),
                    Departamentos = new List<Department>(),
                    Escudos = new List<ShieldMunicipality>(),
                    Themas = new List<Theme>()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveAlcadia(ToAlcaldiaWeb model)
        {
            try
            {
                var municipality = model.Alcaldia;

                if (municipality == null)
                {
                    TempData["Error"] = "Datos inválidos.";
                    return RedirectToAction("FormMunicipalityUpdate", new { Id = 0 });
                }

                if (municipality.Id > 0)
                {
                    var response = await _unitOfWork
                        .genericRepository<Municipality>()
                        .FindAsync_Predicate(x => x.Id == municipality.Id);

                    if (response == null)
                    {
                        TempData["Error"] = "Municipio no encontrado.";
                        return RedirectToAction("FormMunicipalityUpdate", new { Id = municipality.Id });
                    }

                    response.Name = municipality.Name;
                    response.EntityCode = municipality.EntityCode;
                    response.Domain = municipality.Domain;
                    response.UserFintech = municipality.UserFintech;
                    response.PasswordFintech = municipality.PasswordFintech;
                    response.DepartmentId = municipality.DepartmentId;
                    response.ThemeId = municipality.ThemeId;
                    response.IdShield = municipality.IdShield;
                    response.IdBank = municipality.IdBank;
                    response.DataPrivacy = municipality.DataPrivacy;
                    response.DataProcessingPrivacy = municipality.DataProcessingPrivacy;
                    response.IsActive = response.IsActive;
                }
                else
                {
                    municipality.IsActive = true;
                    await _unitOfWork.genericRepository<Municipality>().AddAsync(municipality);
                }

                var rows = await _unitOfWork.SaveChangesAsync();
                TempData["Success"] = "Proceso de guardado - Activado";

                if (rows <= 0)
                {
                    TempData["Error"] = "No se guardaron cambios.";
                    return RedirectToAction("FormMunicipalityUpdate", new { Id = municipality.Id });
                }

                TempData["Success"] = municipality.Id > 0 ? "Municipio actualizado." : "Municipio creado.";
                return RedirectToAction("FormMunicipalityUpdate", new { Id = municipality.Id });
            }
            catch (Exception e)
            {
                TempData["Error"] = $"Error -> {e.Message}";
                return RedirectToAction("FormMunicipalityUpdate", new { Id = model.Alcaldia?.Id ?? 0 });
            }
        }




        [HttpPost]
        public async Task<IActionResult> UpdateStatusMunicipality(int id, bool isActive)
        {
            try
            {
                var response = await _ProcedureServices.UpdateStatusMunicipality(id, isActive);
                if (response.BooleanStatus)
                {
                    TempData["Success"] = "El estado del municipio se actualizó correctamente.";
                }
                else
                {
                    TempData["Error"] = "No se pudo actualizar el estado del municipio.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrió un error inesperado al actualizar el municipio.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> updateMunicipalitySocialMedium(int id, CreateMunicipalitySocialMediumDto updateMunicipalitySocialMediumDto)
        {

            try
            {
                if (updateMunicipalitySocialMediumDto.MunicipalityId <= 0 || updateMunicipalitySocialMediumDto.SocialMediaTypeId <= 0 || string.IsNullOrEmpty(updateMunicipalitySocialMediumDto.Url))
                {
                    TempData["message"] = "no se puedo actulizar la red social del municipio.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("MunicipalitySocialMediaIndex");
                }

                var result = await _ProcedureServices.updateMunicipalitySocialMedium(id, updateMunicipalitySocialMediumDto);

                if (!result.BooleanStatus)
                {
                    TempData["message"] = "no se puedo actulizar la red social del municipio.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("MunicipalitySocialMediaIndex", new { id = updateMunicipalitySocialMediumDto.MunicipalityId });
                }
                else
                {
                    TempData["message"] = "la red social se actualizo correctamente.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("MunicipalitySocialMediaIndex", new { id = updateMunicipalitySocialMediumDto.MunicipalityId });
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = $"no se puedo actulizar la red social del municipio. -> {ex.Message}";
                TempData["MessageType"] = "error";
                return RedirectToAction("MunicipalitySocialMediaIndex", new { id = updateMunicipalitySocialMediumDto.MunicipalityId });
            }
        }


        [HttpGet]
        public async Task<IActionResult> deparmentIndex()
        {
            var respose = await _unitOfWork.genericRepository<Department>().GetAllAsync();
            return View(respose);
        }

        [HttpPost]
        public async Task<IActionResult> Createdeparment(CreateDepartmentDto departmentDto)
        {
            try
            {
                if (string.IsNullOrEmpty(departmentDto.Name))
                {
                    TempData["message"] = "No se puedo crear el departamento, campo vacio.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("deparmentIndex");
                }
                await _departmentService.createDepartment(departmentDto);
                TempData["message"] = "El depatamento fue creado correctamente.";
                TempData["MessageType"] = "success";
                return RedirectToAction("deparmentIndex");

            }
            catch (Exception ex)
            {
                TempData["message"] = "No se puedo crear el departamento.";
                TempData["MessageType"] = "error";
                return RedirectToAction("deparmentIndex");

            }

        }

        [HttpPost]
        public async Task<IActionResult> updatedeparment(int id, CreateDepartmentDto updatedepartmentDto)
        {
            try
            {
                if (string.IsNullOrEmpty(updatedepartmentDto.Name) || id <= 0)
                {
                    TempData["message"] = " los cambos estan vacios";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("deparmentIndex");

                }
                var result = await _departmentService.updateDepartment(id, updatedepartmentDto);
                if (!result.BooleanStatus)
                {
                    TempData["message"] = "no se puedo actulizar el departamento.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("deparmentIndex");


                }
                else
                {
                    TempData["message"] = "El depatamento se actualizo correctamente.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("deparmentIndex");


                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "no sepuedo actualizar el departamento. comunicate con soporte.";
                TempData["MessageType"] = "error";
                return RedirectToAction("deparmentIndex");

            }
        }
    }
}

