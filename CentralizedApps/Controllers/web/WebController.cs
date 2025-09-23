
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers.web
{

    public class WebController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUnitOfWork _unitOfWork;

        public WebController(IWebHostEnvironment env, IUnitOfWork unitOfWork)
        {
            _env = env;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<List<ShieldMunicipality>> GetShieldMunicipalities()
        {
            var response = await _unitOfWork.genericRepository<ShieldMunicipality>().GetAllAsync();
            if (response != null)
            {
                return response.ToList();
            }
            else
            {
                return new List<ShieldMunicipality>();

            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(string municipio, [FromForm] IFormFile archivo)
        {
            if (HttpContext.Request.Method == "GET")
            {
                return View();
            }

            if (archivo == null || archivo.Length == 0)
            {
                ViewBag.Error = "No se subió ninguna imagen";
                return View();
            }

            municipio = municipio.Trim();
            foreach (var c in Path.GetInvalidFileNameChars())
                municipio = municipio.Replace(c, '_');

            string carpetaBase = Path.Combine(_env.WebRootPath, "Uploads");
            string pathCarpeta = Path.Combine(carpetaBase, municipio);

            if (!Directory.Exists(pathCarpeta))
                Directory.CreateDirectory(pathCarpeta);

            var extension = Path.GetExtension(archivo.FileName).ToLower();
            string[] permitidos = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!permitidos.Contains(extension))
            {
                ViewBag.Error = "Tipo de archivo no permitido";
                return View();
            }

            string nombreBase = Path.GetFileNameWithoutExtension(archivo.FileName);
            string nombreArchivo = $"{nombreBase}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";
            string pathArchivo = Path.Combine(pathCarpeta, nombreArchivo);
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

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

            ViewBag.Mensaje = $"Imagen subida correctamente curretly url ${remoteIpAddress}";
            ViewBag.Url = url;

            return View(GetShieldMunicipalities());
        }

        [HttpGet]
        public async Task<IActionResult> SubirImagen()
        {
            return View(GetShieldMunicipalities().Result);
        }
    }
}

