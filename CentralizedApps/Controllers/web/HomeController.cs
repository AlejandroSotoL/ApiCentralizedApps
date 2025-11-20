using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers.web

{

    [Authorize(Roles = "Administrador")]

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
