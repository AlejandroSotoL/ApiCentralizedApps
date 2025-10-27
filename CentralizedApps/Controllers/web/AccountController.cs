using CentralizedApps.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;


namespace CentralizedApps.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _Unit;
        public AccountController(IUnitOfWork Unit)
        {
            _Unit = Unit;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            if (!username.IsNullOrEmpty() && !password.IsNullOrEmpty())
            {
                var response = await _Unit.AuthRepositoryUnitOfWork.LoginAdmins(username, password);
                if (response == null)
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                    return View();
                }

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, response.CompleteName ?? string.Empty),
            new Claim(ClaimTypes.Role, response.IdRolNavigation?.TypeRole ?? string.Empty)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                return Redirect(returnUrl ?? "/");
            }

            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied(string? returnUrl = null)
        {
            return View(returnUrl ?? "Login");
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AddAdmin(string completeName, string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(completeName))
            {
                string convertPASSWORDTO = BCrypt.Net.BCrypt.HashPassword(password);
                var result = await _Unit.AuthRepositoryUnitOfWork.AddAdmin(completeName, username, convertPASSWORDTO);
                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError(string.Empty, "Error al agregar administrador.");
            return View("Login");
        }
    }
}
