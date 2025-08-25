using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers.web
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
