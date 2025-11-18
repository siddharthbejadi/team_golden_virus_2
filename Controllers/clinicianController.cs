using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace team_golden_virus.Controllers
{
    // [Authorize] ensures only logged-in users can access this controller.
    [Authorize]
    public class ClinicianController : Controller
    {
        // Action: /Clinician/Dashboard
        public IActionResult Dashboard()
        {
            // The Clinician Dashboard logic will go here (fetching patient list, calendar, alerts summary)
            return View(); // Looks for Views/Clinician/Dashboard.cshtml
        }

        // You would add actions like PatientDetail() or ScheduleMeeting() here later.
    }
}
