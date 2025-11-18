using System.Collections.Concurrent;
using team_golden_virus.Models;

namespace team_golden_virus.Services
{
    // Very simple in-memory user store (for demo/testing only)
    public class InMemoryUserService : IUserService
    {
        private readonly ConcurrentDictionary<string, (ApplicationUser user, string password)> _users = new();

        public ApplicationUser? ValidateUser(string email, string password)
        {
            if (_users.TryGetValue(email.ToLowerInvariant(), out var info) && info.password == password)
                return info.user;
            return null;
        }

        public ApplicationUser? FindByEmail(string email)
        {
            if (_users.TryGetValue(email.ToLowerInvariant(), out var info))
                return info.user;
            return null;
        }

        public void CreateUser(ApplicationUser user, string password)
        {
            _users[user.Email.ToLowerInvariant()] = (user, password);
        }
    }
}
