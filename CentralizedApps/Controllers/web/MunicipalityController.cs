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
        private readonly ILogger<MunicipalityController> _logger;
        public MunicipalityController(ILogger<MunicipalityController> logger,IMunicipalityServices MunicipalityServices, IProcedureServices ProcedureServices, IUnitOfWork unitOfWork, IDepartmentService departmentService, IWeb web, IBank bank)
        {
            _MunicipalityServices = MunicipalityServices;
            _ProcedureServices = ProcedureServices;
            _unitOfWork = unitOfWork;
            _departmentService = departmentService;
            _web = web;
            _bank = bank;
            _logger = logger;
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

        [HttpPost]
        public IActionResult SelectMunicipalitySocialMedia(int id)
        {
            return RedirectToAction("MunicipalitySocialMediaIndex", "Municipality", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> MunicipalitySocialMediaIndex(int? id)
        {

            var response = await _web.MunicipalitiesAndSocialMediaType(id);
            return View(response);

        }

        [HttpPost]
        public IActionResult SelectCourse(int id)
        {
            return RedirectToAction("CourseIndex", "Municipality", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> CourseIndex(int? id)
        {

            var response = await _web.courses(id);
            return View(response);

        }

        [HttpPost]
        public IActionResult SelectQueryField(int id)
        {
            return RedirectToAction("QueryFieldIndex", "Municipality", new { id });
        }
        [HttpGet]
        public async Task<IActionResult> QueryFieldIndex(int? id)
        {

            var response = await _web.QueryField(id);
            return View(response);

        }

        [HttpPost]
        public IActionResult SelectNewsMunicipality(int id)
        {
            return RedirectToAction("NewsMunicipalityIndex", "Municipality", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> NewsMunicipalityIndex(int? id)
        {

            var response = await _web.NewsMunicipality(id);
            return View(response);

        }

        [HttpPost]
        public IActionResult SelectSportsFacilities(int id)
        {
            return RedirectToAction("SportsFacilitiesIndex", "Municipality", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> SportsFacilitiesIndex(int? id)
        {

            var response = await _web.SportsFacilities(id);
            return View(response);

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


        [HttpGet]
        public async Task<IActionResult> DocomentTypeIndex()
        {
            var respose = await _ProcedureServices.GetDocumentTypes();
            return View(respose);
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

                TempData["message"] = "Se creo el curso correctamente.";
                TempData["MessageType"] = "success";
                return RedirectToAction("CourseIndex");
            }
            catch (Exception ex)
            {
                TempData["message"] = $"no se puedo crear el curso del municipio.";
                TempData["MessageType"] = "error";
                return RedirectToAction("CourseIndex");
            }
        }
        [HttpPost]
        public async Task<IActionResult> createQueryField(QueryFieldDto queryFieldDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "No se pudo crear el campo cosulta, hay campos vacíos o inválidos.";
                TempData["MessageType"] = "error";
                return RedirectToAction("QueryFieldIndex", new { id = queryFieldDto.MunicipalityId });
            }
            try
            {
                if (queryFieldDto == null || queryFieldDto.MunicipalityId <= 0
                    || string.IsNullOrWhiteSpace(queryFieldDto.FieldName)
                    || string.IsNullOrWhiteSpace(queryFieldDto.QueryFieldType))
                {
                    TempData["message"] = "No se pudo crear el campo cosulta, hay campos vacíos o inválidos.";
                    TempData["MessageType"] = "error";

                    return RedirectToAction("QueryFieldIndex", new { id = queryFieldDto?.MunicipalityId });
                }
                await _ProcedureServices.createQueryField(queryFieldDto);

                TempData["message"] = "Se creo el campo cosulta correctamente.";
                TempData["MessageType"] = "success";
                return RedirectToAction("QueryFieldIndex");
            }
            catch (Exception ex)
            {
                TempData["message"] = $"no se puedo crear el campo cosulta del municipio.";
                TempData["MessageType"] = "error";
                return RedirectToAction("QueryFieldIndex");
            }
        }


        [HttpPost]
        public async Task<IActionResult> createNewsMunicipality(NewsByMunicipalityDto newsByMunicipalityDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "No se pudo crear las noticias del municipio, hay campos vacíos o inválidos.";
                TempData["MessageType"] = "error";
                return RedirectToAction("NewsMunicipalityIndex", new { id = newsByMunicipalityDto.IdMunicipality });
            }
            try
            {
                if (newsByMunicipalityDto == null || newsByMunicipalityDto.IdMunicipality <= 0
                    || string.IsNullOrWhiteSpace(newsByMunicipalityDto.GetUrlNew))
                {
                    TempData["message"] = "No se pudo crear las noticias del municipio, hay campos vacíos o inválidos.";
                    TempData["MessageType"] = "error";

                    return RedirectToAction("NewsMunicipalityIndex", new { id = newsByMunicipalityDto?.IdMunicipality });
                }
                await _ProcedureServices.createNewNotice(newsByMunicipalityDto);

                TempData["message"] = "Se creo la noticia del municipio correctamente.";
                TempData["MessageType"] = "success";
                return RedirectToAction("NewsMunicipalityIndex");
            }
            catch (Exception ex)
            {
                TempData["message"] = $"no se puedo crear la noticia del municipio.";
                TempData["MessageType"] = "error";
                return RedirectToAction("NewsMunicipalityIndex");
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
                TempData["message"] = $"no se puedo crear el escenario deportivo del municipio.";
                TempData["MessageType"] = "error";
                return RedirectToAction("SportsFacilitiesIndex");
            }
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
        public async Task<IActionResult> createDocumentType(DocumentTypeDto documentTypeDto)
        {
            try
            {
                if (string.IsNullOrEmpty(documentTypeDto.NameDocument))
                {
                    TempData["message"] = "No se puedo crear el tipo de documento, campo vacio.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("DocomentTypeIndex");
                }
                await _ProcedureServices.createDocumentType(documentTypeDto);
                TempData["message"] = "El tipo de documento fue creado correctamente.";
                TempData["MessageType"] = "success";
                return RedirectToAction("DocomentTypeIndex");

            }
            catch (Exception ex)
            {
                TempData["message"] = "No se puedo crear el tipo de documento.";
                TempData["MessageType"] = "error";
                return RedirectToAction("DocomentTypeIndex");

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
                    TempData["message"] = "El banco se actualizo correctamente.";
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
        public async Task<IActionResult> updateDocumentType(int id, DocumentTypeDto updateDocumentTypeDto)
        {
            try
            {
                if (string.IsNullOrEmpty(updateDocumentTypeDto.NameDocument) || id <= 0)
                {
                    TempData["message"] = " los cambos estan vacios";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("DocomentTypeIndex");

                }
                var result = await _ProcedureServices.updateDocumentType(id, updateDocumentTypeDto);
                if (!result.BooleanStatus)
                {
                    TempData["message"] = "no se puedo actulizar el tipo de documento.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("DocomentTypeIndex");


                }
                else
                {
                    TempData["message"] = "El tipo de documento se actualizo correctamente.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("DocomentTypeIndex");


                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "no sepuedo actualizar el tipo de documento. comunicate con soporte.";
                TempData["MessageType"] = "error";
                return RedirectToAction("DocomentTypeIndex");

            }
        }

        [HttpPost]
        public async Task<IActionResult> updateCourse(int id, CreateCourseDto updateCourseDto)
        {

            try
            {
                if (updateCourseDto.MunicipalityId <= 0 || string.IsNullOrEmpty(updateCourseDto.Get) || string.IsNullOrEmpty(updateCourseDto.Post) || string.IsNullOrEmpty(updateCourseDto.Name))
                {
                    TempData["message"] = "no se puedo Actualizar el curso, campos vacios";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("CourseIndex", new { id = updateCourseDto.MunicipalityId });
                }

                var result = await _web.updateCourse(id, updateCourseDto);

                if (!result.BooleanStatus)
                {
                    TempData["message"] = "no se puedo Actualizar el curso.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("CourseIndex", new { id = updateCourseDto.MunicipalityId });
                }
                else
                {
                    TempData["message"] = "Se Actualizar el curso correctamente.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("CourseIndex", new { id = updateCourseDto.MunicipalityId });
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "no se puedo Actualizar el curso. comunicate con el desarrollador";
                TempData["MessageType"] = "error";
                return RedirectToAction("CourseIndex", new { id = updateCourseDto.MunicipalityId });
            }
        }
        [HttpPost]
        public async Task<IActionResult> updateQueryField(int id, QueryFieldDto queryFieldDto)
        {

            try
            {
                 _logger.LogInformation("Id: {Id}, FieldName: {FieldName}, MunicipalityId: {MunicipalityId}, QueryFieldType: {QueryFieldType}",
        id, queryFieldDto.FieldName, queryFieldDto.MunicipalityId, queryFieldDto.QueryFieldType);

                if (queryFieldDto.MunicipalityId <= 0 || string.IsNullOrEmpty(queryFieldDto.FieldName) || string.IsNullOrEmpty(queryFieldDto.QueryFieldType))
                {
                    TempData["message"] = "no se puedo Actualizar el campo consulta, campos vacios";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("QueryFieldIndex", new { id = queryFieldDto.MunicipalityId });
                }

                var result = await _ProcedureServices.updateQueryField(id, queryFieldDto);

                if (!result.BooleanStatus)
                {
                    TempData["message"] = "no se puedo Actualizar el campo consulta.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("QueryFieldIndex", new { id = queryFieldDto.MunicipalityId });
                }
                else
                {
                    TempData["message"] = "Se Actualizar el campo consulta correctamente.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("QueryFieldIndex", new { id = queryFieldDto.MunicipalityId });
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "no se puedo Actualizar el campo consulta. comunicate con el desarrollador";
                TempData["MessageType"] = "error";
                return RedirectToAction("QueryFieldIndex", new { id = queryFieldDto.MunicipalityId });
            }
        }
        [HttpPost]
        public async Task<IActionResult> updateNewsMunicipality(int id, NewsByMunicipalityDto newsByMunicipalityDto)
        {

            try
            {
                if (newsByMunicipalityDto.IdMunicipality <= 0 || string.IsNullOrEmpty(newsByMunicipalityDto.GetUrlNew))
                {
                    TempData["message"] = "no se puedo Actualizar la noticia del municipio, campos vacios";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("NewsMunicipalityIndex", new { id = newsByMunicipalityDto.IdMunicipality });
                }

                var result = await _ProcedureServices.updateNews(id, newsByMunicipalityDto);

                if (!result.BooleanStatus)
                {
                    TempData["message"] = "no se puedo Actualizar la noticia del municipio.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("NewsMunicipalityIndex", new { id = newsByMunicipalityDto.IdMunicipality });
                }
                else
                {
                    TempData["message"] = "Se Actualizar la noticia del municipio correctamente.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("NewsMunicipalityIndex", new { id = newsByMunicipalityDto.IdMunicipality });
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "no se puedo Actualizar la noticia del municipio. comunicate con el desarrollador";
                TempData["MessageType"] = "error";
                return RedirectToAction("NewsMunicipalityIndex", new { id = newsByMunicipalityDto.IdMunicipality });
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateSportFacilities(int id, CreateSportsFacilityDto updateSportsFacilityDto)
        {

            try
            {
                if (updateSportsFacilityDto.MunicipalityId <= 0 || string.IsNullOrEmpty(updateSportsFacilityDto.Get) || string.IsNullOrEmpty(updateSportsFacilityDto.ReservationPost) || string.IsNullOrEmpty(updateSportsFacilityDto.CalendaryPost) || string.IsNullOrEmpty(updateSportsFacilityDto.Name))
                {
                    TempData["message"] = "no se puedo Actualizar el escenario deportivo, campos vacios";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("SportsFacilitiesIndex", new { id = updateSportsFacilityDto.MunicipalityId });
                }

                var result = await _web.UpdateSportFacilietes(id, updateSportsFacilityDto);

                if (!result.BooleanStatus)
                {
                    TempData["message"] = "no se puedo Actualizar el escenario deportivo.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("SportsFacilitiesIndex", new { id = updateSportsFacilityDto.MunicipalityId });
                }
                else
                {
                    TempData["message"] = "Se Actualizar el escenario deportivo correctamente.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("SportsFacilitiesIndex", new { id = updateSportsFacilityDto.MunicipalityId });
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "no se puedo Actualizar el escenario deportivo. comunicate con el desarrollador";
                TempData["MessageType"] = "error";
                return RedirectToAction("SportsFacilitiesIndex", new { id = updateSportsFacilityDto.MunicipalityId });
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
    }
}

