using team_golden_virus.Models;

namespace team_golden_virus.Services
{
    public interface IUserService
    {
        ApplicationUser? ValidateUser(string email, string password);
        ApplicationUser? FindByEmail(string email);
        void CreateUser(ApplicationUser user, string password);
    }
}
