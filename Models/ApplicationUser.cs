using Microsoft.AspNetCore.Identity;

namespace team_golden_virus.Models
{
    // Minimal ApplicationUser to satisfy Identity usage in controllers.
    // Extend as needed (add properties, claims, etc.).
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
    }
}
