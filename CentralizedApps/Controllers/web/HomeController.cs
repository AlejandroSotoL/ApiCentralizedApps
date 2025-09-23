using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers.web
{
    public class HomeController : Controller
    {

        [Authorize (Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
