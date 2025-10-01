using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using CentralizedApps.Services.ServicesWeb.Interface;
using Microsoft.AspNetCore.Mvc;


namespace CentralizedApps.Controllers.web
{
    public class MunicipalityController : Controller
    {
        private readonly IMunicipalityServices _MunicipalityServices;
        private readonly IProcedureServices _ProcedureServices;
        private readonly IDepartmentService _departmentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWeb _web;
        public MunicipalityController(IMunicipalityServices MunicipalityServices, IProcedureServices ProcedureServices, IUnitOfWork unitOfWork, IDepartmentService departmentService, IWeb web)
        {
            _MunicipalityServices = MunicipalityServices;
            _ProcedureServices = ProcedureServices;
            _unitOfWork = unitOfWork;
            _departmentService = departmentService;
            _web = web;
        }


        // get all municipaly with relations 
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

        // get all municipality social media
        [HttpGet]
        public async Task<IActionResult> MunicipalitySocialMediaIndex(int? id)
        {

            var response = await _web.MunicipalitiesAndSocialMediaType(id);
            return View(response);

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

// get all deparment
        [HttpGet]

        public async Task<IActionResult> deparmentIndex()
        {
            var respose = await _unitOfWork.genericRepository<Department>().GetAllAsync();
            return View(respose);
        }

        // create deparment
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
        // create social type
        [HttpPost]
        public async Task<IActionResult> CreateSocialType(CreateSocialMediaTypeDto createSocialMediaTypeDto)
        {
            try
            {
                if (string.IsNullOrEmpty(createSocialMediaTypeDto.Name))
                {
                    TempData["message"] = "No se puedo crear la red social, campo vacio.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("MunicipalitySocialMediaIndex");
                }
                await _ProcedureServices.createSocialMediaType(createSocialMediaTypeDto);
                TempData["message"] = "La red social fue creada correctamente.";
                TempData["MessageType"] = "success";
                return RedirectToAction("MunicipalitySocialMediaIndex");

            }
            catch (Exception ex)
            {
                TempData["message"] = "No se puedo crear l ared social.";
                TempData["MessageType"] = "error";
                return RedirectToAction("MunicipalitySocialMediaIndex");

            }

        }


        // create municipality social media

        public async Task<IActionResult> CreateMuncipalitySocialMedia(MunicipalitySocialMeditaDto_Response dto)
        {
            if (dto.MunicipalityId <= 0 || dto.SocialMediaTypeId <= 0 || string.IsNullOrEmpty(dto.Url))
            {
                TempData["message"] = "No se puedo crear la red social, campo vacio.";
                TempData["MessageType"] = "error";
                return RedirectToAction("MunicipalitySocialMediaIndex");
            }

            var result = await _ProcedureServices.AddMuncipalitySocialMediaToMunicipality(dto);
            if (!result)
            {
            TempData["message"] = "No se puedo crear la red social.";
            TempData["MessageType"] = "error";
                return RedirectToAction("MunicipalitySocialMediaIndex");
            }

                TempData["message"] = "la red social fue creada correctamente.";
                TempData["MessageType"] = "success";
            return RedirectToAction("MunicipalitySocialMediaIndex");

        }


        // update deparment

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


        [HttpPost]
        public async Task<IActionResult> updateSocialType(int id, CreateSocialMediaTypeDto updateSocialMediaTypeDto)
        {
            try
            {
                if (string.IsNullOrEmpty(updateSocialMediaTypeDto.Name) || id <= 0)
                {
                    TempData["message"] = " los cambos estan vacios";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("MunicipalitySocialMediaIndex");

                }
                var result = await _ProcedureServices.updateSocialMediaType(id, updateSocialMediaTypeDto);
                if (!result.BooleanStatus)
                {
                    TempData["message"] = "no se puedo actulizar la red social.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("MunicipalitySocialMediaIndex");


                }
                else
                {
                    TempData["message"] = "la red social se actualizo correctamente.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("MunicipalitySocialMediaIndex");


                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "no sepuedo actualizar la red social. comunicate con soporte.";
                TempData["MessageType"] = "error";
                return RedirectToAction("deparmentIndex");

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

        // update municipality social medium
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
                    return RedirectToAction("MunicipalitySocialMediaIndex", new {id= updateMunicipalitySocialMediumDto.MunicipalityId } );
                }
                else
                {
                    TempData["message"] = "la red social se actualizo correctamente.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("MunicipalitySocialMediaIndex", new {id= updateMunicipalitySocialMediumDto.MunicipalityId });
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "no se puedo actulizar la red social del municipio.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("MunicipalitySocialMediaIndex", new {id= updateMunicipalitySocialMediumDto.MunicipalityId } );
            }
        }
    }
}