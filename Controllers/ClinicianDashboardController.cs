using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace team_golden_virus.Controllers
{
    [Authorize(Roles = "Clinician")]
    public class ClinicianDashboardController : Controller
    {
        public IActionResult Index()
        {
            var clinicianName = User?.Identity?.Name ?? "Clinician";
            return View(model: clinicianName);
        }
    }
}
