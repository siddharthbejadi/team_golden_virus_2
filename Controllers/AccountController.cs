using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using team_golden_virus.Models;
using team_golden_virus.Services;
using team_golden_virus.ViewModels.Account;

namespace team_golden_virus.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            _logger.LogInformation("Login attempt for '{Email}'", model?.Email);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // First try email sign-in
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            ApplicationUser? user = null;
            if (!result.Succeeded)
            {                // If not by email, try lookup by full name and validate password                user = _userManager.Users.FirstOrDefault(u => u.FullName != null && u.FullName.Equals(model.Email, System.StringComparison.OrdinalIgnoreCase));                if (user != null)                {                    var pwOk = await _userManager.CheckPasswordAsync(user, model.Password);                    if (pwOk)                    {                        await _signInManager.SignInAsync(user, model.RememberMe);                        result = Microsoft.AspNetCore.Identity.SignInResult.Success;                    }                }            }            else            {                user = await _userManager.FindByEmailAsync(model.Email);            }            if (!result.Succeeded || user == null)            {                ModelState.AddModelError(string.Empty, "Invalid credentials.");                return View(model);            }            var roles = await _userManager.GetRolesAsync(user);            var role = roles.FirstOrDefault() ?? "Patient";            _logger.LogInformation("User '{Email}' authenticated as role '{Role}'", user.Email, role);            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))                return Redirect(returnUrl);            return role switch            {                "Admin" => RedirectToAction("Index", "AdminDashboard"),                "Clinician" => RedirectToAction("Index", "ClinicianDashboard"),                _ => RedirectToAction("Index", "PatientDashboard"),            };        }        [HttpGet]        public IActionResult Register()        {            return View();        }        [HttpPost]        [ValidateAntiForgeryToken]        public async Task<IActionResult> Register(RegisterViewModel model)        {            if (!ModelState.IsValid)                return View(model);            var existing = await _userManager.FindByEmailAsync(model.Email);            if (existing != null)            {                ModelState.AddModelError(string.Empty, "Email already registered.");                return View(model);            }            var user = new ApplicationUser            {                Id = System.Guid.NewGuid().ToString(),                Email = model.Email,                UserName = model.Email,                FullName = model.FullName,                UserRole = "Patient"            };            var createResult = await _userManager.CreateAsync(user, model.Password);            if (!createResult.Succeeded)            {                foreach (var error in createResult.Errors)                    ModelState.AddModelError(string.Empty, error.Description);                return View(model);            }            // ensure role exists and assign            await _userManager.AddToRoleAsync(user, "Patient");            return RedirectToAction("Login");        }        [HttpPost]        [ValidateAntiForgeryToken]        public async Task<IActionResult> Logout()        {            await _signInManager.SignOutAsync();            return RedirectToAction("Index", "Home");        }    }}
