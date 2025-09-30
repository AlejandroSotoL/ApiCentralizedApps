using AutoMapper;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
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
        private readonly IMapper _mapper;
        public MunicipalityController(IMunicipalityServices MunicipalityServices, IProcedureServices ProcedureServices, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _MunicipalityServices = MunicipalityServices;
            _ProcedureServices = ProcedureServices;
            _unitOfWork = unitOfWork;
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
            catch (Exception ex)
            {
                return View(new List<GetMunicipalitysDto>());
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
                    TempData["Error"] = "Datos inv�lidos, verifica la informaci�n.";
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
        public async Task<IActionResult> UpdateStatusMunicipality(int id, bool isActive)
        {
            try
            {
                var response = await _ProcedureServices.UpdateStatusMunicipality(id, isActive);
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