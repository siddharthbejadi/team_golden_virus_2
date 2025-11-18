namespace team_golden_virus.Models
{
    public class Alert
    {
        public string Severity { get; set; }
        public string Message { get; set; }
        public System.DateTime Timestamp { get; set; }
        // optional: other properties like Id, PatientId etc.
    }
}
