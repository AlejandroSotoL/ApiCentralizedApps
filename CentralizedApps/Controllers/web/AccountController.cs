using CentralizedApps.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CentralizedApps.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _Unit;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUnitOfWork Unit, ILogger<AccountController> logger)
        {
            _Unit = Unit;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [EnableRateLimiting("LoginPolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            if (!username.IsNullOrEmpty())
            {
                var response = await _Unit.AuthRepositoryUnitOfWork.LoginAdmins(username, password);
                if (response == null)
                {
                    // Audit log: failed login attempt
                    _logger.LogWarning("Failed login attempt for user: {Username} from IP: {IP}",
                        username, HttpContext.Connection.RemoteIpAddress);

                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, response.CompleteName ?? string.Empty),
                    new Claim(ClaimTypes.Role, response.IdRolNavigation?.TypeRole ?? string.Empty),
                    new Claim("UserId", response.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(45)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                // Audit log: successful login
                _logger.LogInformation("User logged in: {Username} (ID: {UserId}) from IP: {IP}",
                    username, response.Id, HttpContext.Connection.RemoteIpAddress);

                return Redirect(returnUrl ?? "/");
            }

            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity?.Name;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Audit log: logout
            _logger.LogInformation("User logged out: {Username}", userName);

            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied(string? returnUrl = null)
        {
            _logger.LogWarning("Access denied for user: {Username} attempting to access: {Path}",
                User.Identity?.Name, returnUrl);
            return View(returnUrl ?? "Login");
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin(string completeName, string username, string password)
        {
            // Password policy validation
            var passwordErrors = ValidatePasswordPolicy(password);
            if (passwordErrors.Any())
            {
                foreach (var error in passwordErrors)
                {
                    ModelState.AddModelError("Password", error);
                }
                TempData["Error"] = string.Join(" ", passwordErrors);
                return RedirectToAction("Index", "Home");
            }

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(completeName))
            {
                string convertPASSWORDTO = BCrypt.Net.BCrypt.HashPassword(password);
                var result = await _Unit.AuthRepositoryUnitOfWork.AddAdmin(completeName, username, convertPASSWORDTO);
                if (result)
                {
                    // Audit log: admin created
                    _logger.LogInformation("New admin created: {Username} by {CreatedBy}",
                        username, User.Identity?.Name);

                    TempData["Success"] = "Administrador creado exitosamente.";
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError(string.Empty, "Error al agregar administrador.");
            TempData["Error"] = "Error al agregar administrador.";
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Validates password against security policy
        /// </summary>
        private List<string> ValidatePasswordPolicy(string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(password))
            {
                errors.Add("La contraseña es requerida.");
                return errors;
            }

            if (password.Length < 8)
            {
                errors.Add("La contraseña debe tener al menos 8 caracteres.");
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                errors.Add("La contraseña debe contener al menos una letra mayúscula.");
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                errors.Add("La contraseña debe contener al menos una letra minúscula.");
            }

            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                errors.Add("La contraseña debe contener al menos un número.");
            }

            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?"":{}|<>]"))
            {
                errors.Add("La contraseña debe contener al menos un carácter especial.");
            }

            return errors;
        }
    }
}
