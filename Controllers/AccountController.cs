using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using team_golden_virus.Models;
using team_golden_virus.Services;
using team_golden_virus.ViewModels.Account;

namespace team_golden_virus.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(); // Views/Account/Login.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userService.ValidateUser(model.Email, model.Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? user.Email),
                new Claim(ClaimTypes.Name, user.FullName ?? user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole ?? "Patient")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // Role-based redirect
            return user.UserRole switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Clinician" => RedirectToAction("Dashboard", "Clinician"),
                _ => RedirectToAction("Dashboard", "Patient"),
            };
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(); // Views/Account/Register.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_userService.FindByEmail(model.Email) != null)
            {
                ModelState.AddModelError(string.Empty, "Email already registered.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                Id = System.Guid.NewGuid().ToString(),
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                UserRole = "Patient"
            };

            _userService.CreateUser(user, model.Password);

            // After registration, redirect to login
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}