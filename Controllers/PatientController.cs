using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using team_golden_virus.Models;
using team_golden_virus.ViewModels;

namespace team_golden_virus.Controllers
{
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;

        public PatientController(ILogger<PatientController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Added Dashboard action to serve Views/Patient/Dashboard.cshtml
        public IActionResult Dashboard()
        {
            var viewModel = new PatientDashboardViewModel
            {
                PatientName = "Sarah Johnson",
                PatientId = "PT-2847",
                ClinicianName = "Dr. Michael Chen",
                ClinicianSpecialty = "Podiatrist & Wound Care Specialist",
                ClinicianEmail = "m.chen@hospital.com",
                NextAppointment = new System.DateTime(2025, 11, 18, 14, 30, 0),
                ActiveAlerts = new System.Collections.Generic.List<Alert>
                {
                    new Alert { Severity = "critical", Message = "High pressure detected on left heel", Timestamp = System.DateTime.Now.AddMinutes(-2) },
                    new Alert { Severity = "warning", Message = "You have been sitting for 45 minutes", Timestamp = System.DateTime.Now.AddMinutes(-12) }
                },
                PressureHistory = new System.Collections.Generic.List<PressureDataPoint>
                {
                    new PressureDataPoint { Time = "16:00", Pressure = 90, Threshold = 140 },
                    new PressureDataPoint { Time = "16:15", Pressure = 110, Threshold = 140 },
                    new PressureDataPoint { Time = "16:30", Pressure = 152, Threshold = 140 },
                    new PressureDataPoint { Time = "16:45", Pressure = 120, Threshold = 140 },
                    new PressureDataPoint { Time = "17:00", Pressure = 85, Threshold = 140 }
                },
                HeatmapZones = new System.Collections.Generic.List<HeatmapZone>
                {
                    new HeatmapZone { Label = "Toes", Pressure = 85 },
                    new HeatmapZone { Label = "Metatarsal", Pressure = 45 },
                    new HeatmapZone { Label = "Arch", Pressure = 30 },
                    new HeatmapZone { Label = "Heel", Pressure = 92 }
                }
            };

            return View("Dashboard", viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
