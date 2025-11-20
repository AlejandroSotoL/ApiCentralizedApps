using AutoMapper;
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
        private readonly IBank _bank;
        private readonly IWeb _web;
        private readonly IMapper _mapper;
        private readonly ILogger<MunicipalityController> _logger;
        public MunicipalityController(ILogger<MunicipalityController> logger, IMunicipalityServices MunicipalityServices, IProcedureServices ProcedureServices, IUnitOfWork unitOfWork, IDepartmentService departmentService, IWeb web, IBank bank, IMapper mapper)
        {
            _MunicipalityServices = MunicipalityServices;
            _ProcedureServices = ProcedureServices;
            _unitOfWork = unitOfWork;
            _departmentService = departmentService;
            _web = web;
            _bank = bank;
            _logger = logger;
            _mapper = mapper;
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

        [HttpGet]
        public async Task<IActionResult> GraphicThemes()
        {
            return View(await _unitOfWork.genericRepository<Theme>().GetAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> FilterGraphicThemes(string? filter)
        {
            if (filter == null)
            {
                TempData["Error"] = "Identificador erroneo";
            }
            var response = await _unitOfWork
                .genericRepository<Theme>()
                .GetAllAsync();

            if (response == null || !response.Any())
            {
                return View("GraphicThemes", new List<Theme>());
            }

            var filterCreated = response
                .Where(x => x.NameTheme == filter)
                .ToList();

            return View("GraphicThemes", filterCreated);
        }


        [HttpPost]
        public async Task<IActionResult> CreateGraphicThemes(Theme theme)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Datos inv lidos, verifica la informaci n.";
                    return RedirectToAction(nameof(GraphicThemes));
                }


                theme.BackGroundColor = ConvertToAndroidColor(theme.BackGroundColor);
                theme.PrimaryColor = ConvertToAndroidColor(theme.PrimaryColor);
                theme.SecondaryColor = ConvertToAndroidColor(theme.SecondaryColor);
                theme.SecondaryColorBlack = ConvertToAndroidColor(theme.SecondaryColorBlack);
                theme.OnPrimaryColorLight = ConvertToAndroidColor(theme.OnPrimaryColorLight);
                theme.OnPrimaryColorDark = ConvertToAndroidColor(theme.OnPrimaryColorDark);

                await _unitOfWork.genericRepository<Theme>().AddAsync(theme);
                await _unitOfWork.SaveChangesAsync();
                TempData["Success"] = "Tema creado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Tenemos problemas -> ${ex.Message}";
            }
            return RedirectToAction(nameof(GraphicThemes));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGraphicTheme(int Id, Theme theme)
        {
            try
            {
                if (theme == null || Id <= 0)
                {
                    TempData["Error"] = $"Tenemos problemas -> El tema es invalido - Id {Id} - Theme {theme?.NameTheme}";
                    return RedirectToAction(nameof(GraphicThemes));
                }

                theme.BackGroundColor = ConvertToAndroidColor(theme.BackGroundColor);
                theme.PrimaryColor = ConvertToAndroidColor(theme.PrimaryColor);
                theme.SecondaryColor = ConvertToAndroidColor(theme.SecondaryColor);
                theme.SecondaryColorBlack = ConvertToAndroidColor(theme.SecondaryColorBlack);
                theme.OnPrimaryColorLight = ConvertToAndroidColor(theme.OnPrimaryColorLight);
                theme.OnPrimaryColorDark = ConvertToAndroidColor(theme.OnPrimaryColorDark);

                var themeDto = _mapper.Map<ThemeDto>(theme);
                var response = await _ProcedureServices.UpdateTheme(Id, themeDto);

                if (response == null || response.BooleanStatus == false)
                {
                    TempData["Error"] = $"No se pudo actualizar el tema con id {Id}. " +
                                        $"{response?.SentencesError}";
                }
                else
                {
                    TempData["Success"] = $"Tema actualizado correctamente. " +
                                        $"{response.SentencesError}";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Tenemos problemas -> {ex.Message}";
            }

            return RedirectToAction(nameof(GraphicThemes));
        }


        private string ConvertToAndroidColor(string? color)
        {
            if (string.IsNullOrEmpty(color)) return color ?? string.Empty;
            return color.StartsWith("#")
                ? color.Replace("#", "0xFF")
                : color;
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
                    response.Latitude = municipality.Latitude;
                    response.Longitude = municipality.Longitude;
                    response.EmailMunicipalities = municipality.EmailMunicipalities;
                    response.EmailPanic = municipality.EmailPanic;
                    response.Phone = municipality.Phone;
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