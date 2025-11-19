using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using team_golden_virus.Services;

namespace team_golden_virus.Controllers
{
    // Development-only debug endpoints to inspect in-memory users and validate credentials.
    [ApiController]
    [Route("/debug")]
    public class DebugController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _env;

        public DebugController(IUserService userService, IWebHostEnvironment env)
        {
            _userService = userService;
            _env = env;
        }

        private IActionResult? ForbidIfNotDevelopment()
        {
            if (!_env.IsDevelopment())
                return Forbid();
            return null;
        }

        [HttpGet("users")]
        public IActionResult Users()
        {
            if (!_env.IsDevelopment())
                return Forbid();

            var inMemory = _userService as InMemoryUserService;
            if (inMemory == null)
                return BadRequest(new { error = "IUserService is not InMemoryUserService" });

            var users = inMemory.GetUsers().Select(u => new { u.Email, u.FullName, u.UserRole }).ToList();
            return Ok(users);
        }

        [HttpGet("validate")]
        public IActionResult Validate(string input, string password)
        {
            if (!_env.IsDevelopment())
                return Forbid();

            var user = _userService.ValidateUser(input, password);
            if (user == null)
                return Ok(new { valid = false });
            return Ok(new { valid = true, user = new { user.Email, user.FullName, user.UserRole } });
        }
    }
}