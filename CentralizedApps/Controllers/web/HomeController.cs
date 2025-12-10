using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers.web
{
    [Authorize(Roles = "Administrador")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index()
        {
            // Get counts for dashboard statistics
            var municipalities = await _unitOfWork.genericRepository<Municipality>().GetAllAsync();
            var themes = await _unitOfWork.genericRepository<Theme>().GetAllAsync();
            var shields = await _unitOfWork.genericRepository<ShieldMunicipality>().GetAllAsync();
            var departments = await _unitOfWork.genericRepository<Department>().GetAllAsync();
            var banks = await _unitOfWork.genericRepository<Bank>().GetAllAsync();

            ViewBag.MunicipalitiesCount = municipalities?.Count() ?? 0;
            ViewBag.ThemesCount = themes?.Count() ?? 0;
            ViewBag.ShieldsCount = shields?.Count() ?? 0;
            ViewBag.DepartmentsCount = departments?.Count() ?? 0;
            ViewBag.BanksCount = banks?.Count() ?? 0;
            ViewBag.ActiveMunicipalities = municipalities?.Count(m => m.IsActive == true) ?? 0;

            return View();
        }
    }
}
