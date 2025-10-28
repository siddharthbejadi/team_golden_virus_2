using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;


namespace team_golden_virus.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public string Welcome()
                {
            return "This is the Welcome action method...";
        }
    }
}
