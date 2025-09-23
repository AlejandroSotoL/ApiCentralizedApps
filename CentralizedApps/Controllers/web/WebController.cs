
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

        // Mostrar el formulario
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> SubirImagen(string municipio, [FromForm] IFormFile archivo)
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

            // Sanitizar municipio
            municipio = municipio.Trim();
            foreach (var c in Path.GetInvalidFileNameChars())
                municipio = municipio.Replace(c, '_');

            // Carpeta base fuera de wwwroot
            string carpetaBase = Path.Combine(_env.WebRootPath, "Uploads");
            string pathCarpeta = Path.Combine(carpetaBase, municipio);

            if (!Directory.Exists(pathCarpeta))
                Directory.CreateDirectory(pathCarpeta);

            // Validar extensión
            var extension = Path.GetExtension(archivo.FileName).ToLower();
            string[] permitidos = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!permitidos.Contains(extension))
            {
                ViewBag.Error = "Tipo de archivo no permitido";
                return View();
            }

            // Nombre único
            string nombreBase = Path.GetFileNameWithoutExtension(archivo.FileName);
            string nombreArchivo = $"{nombreBase}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";
            string pathArchivo = Path.Combine(pathCarpeta, nombreArchivo);
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Guardar en disco
            using (var stream = new FileStream(pathArchivo, FileMode.Create))
                await archivo.CopyToAsync(stream);

            //  URL 
            string url = $"{Request.Scheme}://{Request.Host}/uploads/{municipio}/{nombreArchivo}";

            // Guardar en DB
            var shieldMunicipality = new ShieldMunicipality
            {
                NameOfMunicipality = municipio,
                Url = url,
            };

            await _unitOfWork.genericRepository<ShieldMunicipality>().AddAsync(shieldMunicipality);
            await _unitOfWork.SaveChangesAsync();

            ViewBag.Mensaje = $"Imagen subida correctamente curretly url ${remoteIpAddress}";
            ViewBag.Url = url;

            return View();
        }

    }
}

