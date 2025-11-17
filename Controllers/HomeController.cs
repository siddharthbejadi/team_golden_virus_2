using Microsoft.AspNetCore.Mvc;
using team_golden_virus.Models;

namespace team_golden_virus.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
