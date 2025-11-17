using team_golden_virus.Models; // use the project's Models namespace
using System.Collections.Generic;
using System;

namespace team_golden_virus.ViewModels
{
    // A single data point for the pressure chart
    public class PressureDataPoint
    {
        public string Time { get; set; }
        public double Pressure { get; set; }
        public double Threshold { get; set; }
    }

    // A class for the simplified heatmap zones
    public class HeatmapZone
    {
        public string Label { get; set; }
        public int Pressure { get; set; }
    }

    // The main ViewModel to hold all data for the dashboard
    public class PatientDashboardViewModel
    {
        // Patient Info (from Header)
        public string PatientName { get; set; }
        public string PatientId { get; set; }

        // Clinician Info (from DoctorContact)
        public string ClinicianName { get; set; }
        public string ClinicianSpecialty { get; set; }
        public string ClinicianEmail { get; set; }
        public DateTime NextAppointment { get; set; }

        // Alerts (from AlertsPanel)
        public List<Alert> ActiveAlerts { get; set; } // 'Alert' now defined in Models

        // Pressure Chart (from PressureChart)
        public List<PressureDataPoint> PressureHistory { get; set; }

        // Heatmap (from FootHeatmap)
        // We can use the simplified zones from your React file
        public List<HeatmapZone> HeatmapZones { get; set; }

        // --- OR ---
        // We can use the 32x32 grid from your project brief
        // public int[,] PressureGrid { get; set; } 
    }
}