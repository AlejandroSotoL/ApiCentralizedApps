using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace CentralizedApps.Controllers.Web
{
    [Authorize(Roles = "Administrador")]
    public class WebController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUnitOfWork _unitOfWork;

        public WebController(IWebHostEnvironment env, IUnitOfWork unitOfWork)
        {
            _env = env;
            _unitOfWork = unitOfWork;
        }

        public IActionResult EnDesarrollo()
        {
            return View();
        }

        private async Task<List<ShieldMunicipality>> GetShieldMunicipalitiesAsync()
        {
            var response = await _unitOfWork
                .genericRepository<ShieldMunicipality>()
                .GetAllAsync();
            return response?.ToList() ?? new List<ShieldMunicipality>();
        }

        [HttpGet]
        public async Task<IActionResult> SubirImagen()
        {
            var municipios = await GetShieldMunicipalitiesAsync();
            return View(municipios);
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarSeleccionados(List<int> selectedIds)
        {
            if (selectedIds == null || !selectedIds.Any())
            {
                ViewBag.Error = "No seleccionaste ningún registro.";
                return View("SubirImagen");
            }

            foreach (var id in selectedIds)
            {
                var entity = await _unitOfWork
                    .genericRepository<ShieldMunicipality>()
                    .GetByIdAsync(id);

                if (entity != null)
                {
                    _unitOfWork.genericRepository<ShieldMunicipality>().Delete(entity);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            ViewBag.Mensaje = $"{selectedIds.Count} registros procesados correctamente.";
            return View("SubirImagen");
        }


        [HttpPost]
        public async Task<IActionResult> AddImage(string municipio, IFormFile archivo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(municipio))
                {
                    ModelState.AddModelError("municipio", "El nombre del municipio es obligatorio.");
                }

                if (archivo == null || archivo.Length == 0)
                {
                    ModelState.AddModelError("archivo", "Debes seleccionar una imagen.");
                }

                if (!ModelState.IsValid)
                {
                    var municipios = await GetShieldMunicipalitiesAsync();
                    return View(municipios);
                }

                municipio = municipio.Trim();
                foreach (var c in Path.GetInvalidFileNameChars())
                    municipio = municipio.Replace(c, '_');

                string carpetaBase = Path.Combine(_env.WebRootPath, "Uploads");
                string pathCarpeta = Path.Combine(carpetaBase, municipio);

                if (!Directory.Exists(pathCarpeta))
                    Directory.CreateDirectory(pathCarpeta);

                var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
                string[] permitidos = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                if (!permitidos.Contains(extension))
                {
                    ModelState.AddModelError("archivo", "Tipo de archivo no permitido.");
                    var municipios = await GetShieldMunicipalitiesAsync();
                    return View(municipios);
                }

                string nombreBase = Path.GetFileNameWithoutExtension(archivo.FileName);
                string nombreArchivo = $"{nombreBase}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";
                string pathArchivo = Path.Combine(pathCarpeta, nombreArchivo);

                using (var stream = new FileStream(pathArchivo, FileMode.Create))
                    await archivo.CopyToAsync(stream);

                string url = $"{Request.Scheme}://{Request.Host}/uploads/{municipio}/{nombreArchivo}";

                var shieldMunicipality = new ShieldMunicipality
                {
                    NameOfMunicipality = municipio,
                    Url = url,
                };

                await _unitOfWork.genericRepository<ShieldMunicipality>().AddAsync(shieldMunicipality);
                await _unitOfWork.SaveChangesAsync();

                TempData["Mensaje"] = "Imagen subida correctamente.";
                TempData["Url"] = url;

                return RedirectToAction(nameof(SubirImagen));
            }
            catch (Exception e)
            {
                return BadRequest(new ValidationResponseDto
                {
                    SentencesError = $"Tenemos problemas para almacenar la imagen ${e.Message}"
                });
            }
        }
    }
}
