using System.Collections.Concurrent;
using team_golden_virus.Models;
using System.Collections.Generic;

namespace team_golden_virus.Services
{
    // Very simple in-memory user store (for demo/testing only)
    public class InMemoryUserService : IUserService
    {
        private readonly ConcurrentDictionary<string, (ApplicationUser user, string password)> _users = new();

        public InMemoryUserService()
        {
            // Seed a default admin user for immediate testing
            var admin = new ApplicationUser
            {
                Id = System.Guid.NewGuid().ToString(),
                Email = "siddarthbejadi@fakemail.com",
                UserName = "siddarthbejadi@fakemail.com",
                FullName = "siddarth",
                UserRole = "Admin"
            };

            // Default admin password: Admin@123 (change as needed)
            _users[admin.Email.ToLowerInvariant()] = (admin, "Admin@123");

            // Seed two clinicians
            var clinician1 = new ApplicationUser
            {
                Id = System.Guid.NewGuid().ToString(),
                Email = "manmeet12@fakemail.com",
                UserName = "manmeet12@fakemail.com",
                FullName = "Manmeeth Singh",
                UserRole = "Clinician"
            };
            var clinician2 = new ApplicationUser
            {
                Id = System.Guid.NewGuid().ToString(),
                Email = "harneetkaur12@fakemail.com",
                UserName = "harneetkaur12@fakemail.com",
                FullName = "Harneet Kaur",
                UserRole = "Clinician"
            };

            // Default clinician password: Clinician@123 (change as needed)
            _users[clinician1.Email.ToLowerInvariant()] = (clinician1, "Clinician@123");
            _users[clinician2.Email.ToLowerInvariant()] = (clinician2, "Clinician@123");
        }

        public ApplicationUser? ValidateUser(string emailOrName, string password)
        {
            if (string.IsNullOrWhiteSpace(emailOrName))
                return null;

            // Normalize input: trim, lower-case, remove accidental commas/spaces around @
            var normalizedInput = emailOrName.Trim().Replace(",", string.Empty);
            var key = normalizedInput.ToLowerInvariant();

            // Try direct email/username lookup first
            if (_users.TryGetValue(key, out var info) && info.password == password)
                return info.user;

            // If not found by email, try matching by full name (case-insensitive, normalized)
            foreach (var entry in _users.Values)
            {
                var storedName = (entry.user.FullName ?? string.Empty).Trim();
                if (string.Equals(storedName, emailOrName.Trim(), System.StringComparison.OrdinalIgnoreCase) && entry.password == password)
                    return entry.user;
            }

            return null;
        }

        public ApplicationUser? FindByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var key = email.Trim().Replace(",", string.Empty).ToLowerInvariant();
            if (_users.TryGetValue(key, out var info))
                return info.user;
            return null;
        }

        public void CreateUser(ApplicationUser user, string password)
        {
            _users[user.Email.ToLowerInvariant()] = (user, password);
        }

        // Development helper to return seeded users (email, fullname, role)
        public IEnumerable<ApplicationUser> GetUsers()
        {
            foreach (var entry in _users.Values)
            {
                yield return entry.user;
            }
        }
    }
}
