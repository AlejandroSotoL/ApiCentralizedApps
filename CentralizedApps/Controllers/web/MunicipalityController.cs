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
        private readonly IBank _bank;
        private readonly IWeb _web;
        public MunicipalityController(IMunicipalityServices MunicipalityServices, IProcedureServices ProcedureServices, IUnitOfWork unitOfWork, IDepartmentService departmentService, IWeb web, IBank bank)
        {
            _MunicipalityServices = MunicipalityServices;
            _ProcedureServices = ProcedureServices;
            _unitOfWork = unitOfWork;
            _departmentService = departmentService;
            _web = web;
            _bank = bank;
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
        public async Task<IActionResult> CourseIndex(int? id)
        {

            var response = await _web.courses(id);
            return View(response);

        }
        [HttpGet]
        public async Task<IActionResult> SportsFacilitiesIndex(int? id)
        {

            var response = await _web.SportsFacilities(id);
            return View(response);

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
        [HttpPost]
        public async Task<IActionResult> createCourse(CreateCourseDto createCourseDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "No se pudo crear el curso, hay campos vacíos o inválidos.";
                TempData["MessageType"] = "error";
                return RedirectToAction("CourseIndex", new { id = createCourseDto.MunicipalityId });
            }
            try
            {
                if (createCourseDto.MunicipalityId == null || createCourseDto.MunicipalityId <= 0
                    || string.IsNullOrWhiteSpace(createCourseDto.Get)
                    || string.IsNullOrWhiteSpace(createCourseDto.Post)
                    || string.IsNullOrWhiteSpace(createCourseDto.Name))
                {
                    TempData["message"] = "No se pudo crear el curso, hay campos vacíos o inválidos.";
                    TempData["MessageType"] = "error";

                    return RedirectToAction("CourseIndex", new { id = createCourseDto.MunicipalityId });
                }
                await _ProcedureServices.createCourse(createCourseDto);

                TempData["message"] = "Se creo en curso correctamente.";
                TempData["MessageType"] = "success";
                return RedirectToAction("CourseIndex");
            }
            catch (Exception ex)
            {
                TempData["message"] = $"no se puedo actulizar la curso del municipio.";
                TempData["MessageType"] = "error";
                return RedirectToAction("CourseIndex");
            }
        }
        [HttpPost]
        public async Task<IActionResult> createSportsFacilities(CreateSportsFacilityDto createSportsFacilityDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "No se pudo crear el escenario deportivo, hay campos vacíos o inválidos.";
                TempData["MessageType"] = "error";
                return RedirectToAction("SportsFacilitiesIndex", new { id = createSportsFacilityDto.MunicipalityId });
            }
            try
            {
                if (createSportsFacilityDto.MunicipalityId == null || createSportsFacilityDto.MunicipalityId <= 0
                    || string.IsNullOrWhiteSpace(createSportsFacilityDto.Get)
                    || string.IsNullOrWhiteSpace(createSportsFacilityDto.ReservationPost)
                    || string.IsNullOrWhiteSpace(createSportsFacilityDto.CalendaryPost)
                    || string.IsNullOrWhiteSpace(createSportsFacilityDto.Name))
                {
                    TempData["message"] = "No se pudo crear el escenario deportivo, hay campos vacíos o inválidos.";
                    TempData["MessageType"] = "error";

                    return RedirectToAction("SportsFacilitiesIndex", new { id = createSportsFacilityDto.MunicipalityId });
                }
                await _ProcedureServices.createSportsFacility(createSportsFacilityDto);

                TempData["message"] = "Se creo el escenario deportivo correctamente.";
                TempData["MessageType"] = "success";
                return RedirectToAction("SportsFacilitiesIndex");
            }
            catch (Exception ex)
            {
                TempData["message"] = $"no se puedo actulizar el escenario deportivo del municipio.";
                TempData["MessageType"] = "error";
                return RedirectToAction("SportsFacilitiesIndex");
            }
        }




        [HttpGet]
        public async Task<IActionResult> deparmentIndex()
        {
            var respose = await _unitOfWork.genericRepository<Department>().GetAllAsync();
            return View(respose);
        }
        [HttpGet]
        public async Task<IActionResult> BankIndex()
        {
            var respose = await _unitOfWork.genericRepository<Bank>().GetAllAsync();
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
        public async Task<IActionResult> createBank(CreateBankDto bankAccountDto)
        {
            try
            {
                if (string.IsNullOrEmpty(bankAccountDto.NameBank))
                {
                    TempData["message"] = "No se puedo crear el banco, campo vacio.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("BankIndex");
                }
                await _bank.CreateBank(bankAccountDto);
                TempData["message"] = "El banco fue creado correctamente.";
                TempData["MessageType"] = "success";
                return RedirectToAction("BankIndex");

            }
            catch (Exception ex)
            {
                TempData["message"] = "No se puedo crear el banco.";
                TempData["MessageType"] = "error";
                return RedirectToAction("BankIndex");

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


        [HttpPost]
        public async Task<IActionResult> updateBank(int id, CreateBankDto bankAccountDto)
        {
            try
            {
                if (string.IsNullOrEmpty(bankAccountDto.NameBank) || id <= 0)
                {
                    TempData["message"] = " los cambos estan vacios";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("BankIndex");

                }
                var result = await _bank.updateBank(id, bankAccountDto);
                if (!result.BooleanStatus)
                {
                    TempData["message"] = "no se puedo actulizar el banco.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("BankIndex");


                }
                else
                {
                    TempData["message"] = "El depatamento se actualizo banco.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("BankIndex");


                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "no sepuedo actualizar el banco. comunicate con soporte.";
                TempData["MessageType"] = "error";
                return RedirectToAction("BankIndex");

            }
        }

        [HttpPost]
        public async Task<IActionResult> updateCourse(int id, CreateCourseDto updateCourseDto)
        {

            try
            {
                if (updateCourseDto.MunicipalityId <= 0 || string.IsNullOrEmpty(updateCourseDto.Get) || string.IsNullOrEmpty(updateCourseDto.Post) || string.IsNullOrEmpty(updateCourseDto.Name))
                {
                    TempData["message"] = "no se puedo crear el curso, campos vacios";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("CourseIndex", new { id = updateCourseDto.MunicipalityId });
                }

                var result = await _web.updateCourse(id, updateCourseDto);

                if (!result.BooleanStatus)
                {
                    TempData["message"] = "no se puedo crear el curso.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("CourseIndex", new { id = updateCourseDto.MunicipalityId });
                }
                else
                {
                    TempData["message"] = "Se creo en curso correctamente.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("CourseIndex", new { id = updateCourseDto.MunicipalityId });
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "no se puedo crear el curso. comunicate con el desarrollador";
                TempData["MessageType"] = "error";
                return RedirectToAction("CourseIndex", new { id = updateCourseDto.MunicipalityId });
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateSportFacilities(int id, CreateSportsFacilityDto updateSportsFacilityDto)
        {

            try
            {
                if (updateSportsFacilityDto.MunicipalityId <= 0 || string.IsNullOrEmpty(updateSportsFacilityDto.Get) || string.IsNullOrEmpty(updateSportsFacilityDto.ReservationPost) || string.IsNullOrEmpty(updateSportsFacilityDto.CalendaryPost) || string.IsNullOrEmpty(updateSportsFacilityDto.Name))
                {
                    TempData["message"] = "no se puedo crear el escenario deportivo, campos vacios";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("SportsFacilitiesIndex", new { id = updateSportsFacilityDto.MunicipalityId });
                }

                var result = await _web.UpdateSportFacilietes(id, updateSportsFacilityDto);

                if (!result.BooleanStatus)
                {
                    TempData["message"] = "no se puedo crear el escenario deportivo.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("SportsFacilitiesIndex", new { id = updateSportsFacilityDto.MunicipalityId });
                }
                else
                {
                    TempData["message"] = "Se creo el escenario deportivo correctamente.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("SportsFacilitiesIndex", new { id = updateSportsFacilityDto.MunicipalityId });
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "no se puedo crear el escenario deportivo. comunicate con el desarrollador";
                TempData["MessageType"] = "error";
                return RedirectToAction("SportsFacilitiesIndex", new { id = updateSportsFacilityDto.MunicipalityId });
            }
        }
    }
}

