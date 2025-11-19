using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using team_golden_virus.Models;

namespace team_golden_virus.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        // Add DbSets for your domain models if needed, for example:
        // public DbSet<Alert> Alerts { get; set; }
    }
}
