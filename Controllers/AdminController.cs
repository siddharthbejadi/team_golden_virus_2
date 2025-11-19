using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace team_golden_virus.Controllers
{
    // [Authorize] ensures only logged-in users can access this controller.
    // For enhanced security, you might add: [Authorize(Roles = "Admin")]
    [Authorize]
    public class AdminController : Controller
    {
        // Action: /Admin/Dashboard
        public IActionResult Dashboard()
        {
            // The Admin Dashboard logic will go here (showing user management links, support tickets)
            return View(); // Looks for Views/Admin/Dashboard.cshtml
        }

        // You would add actions like CreateUser() or ManageTickets() here later.
    }

    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
