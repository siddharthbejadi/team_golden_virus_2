using Microsoft.AspNetCore.Mvc;

namespace team_golden_virus.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Views/Home/Index.cshtml
        }

        public IActionResult Privacy()
        {
            return View(); // Views/Home/Privacy.cshtml
        }

        // Optional error action if layout uses it
        public IActionResult Error()
        {
            return View();
        }
    }
}
