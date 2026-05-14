using Sponsorship.Application.Interfaces;
using Sponsorship.Domain.Entities;
using Sponsorship.Domain.Enums;

namespace Sponsorship.Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            IPasswordHasher passwordHasher)
        {
            if (context.Users.Any())
            {
                return;
            }

            var users = new List<User>
            {
                new()
                {
                    Name = "Requestor User",
                    Email = "requestor@test.com",
                    PasswordHash = passwordHasher.Hash("Test123!"),
                    Role = UserRole.Requestor
                },

                new()
                {
                    Name = "Manager User",
                    Email = "manager@test.com",
                    PasswordHash = passwordHasher.Hash("Test123!"),
                    Role = UserRole.Manager
                },

                new()
                {
                    Name = "Finance User",
                    Email = "finance@test.com",
                    PasswordHash = passwordHasher.Hash("Test123!"),
                    Role = UserRole.FinanceAdmin
                },

                new()
                {
                    Name = "System Admin",
                    Email = "admin@test.com",
                    PasswordHash = passwordHasher.Hash("Test123!"),
                    Role = UserRole.SystemAdmin
                }
            };

            await context.Users.AddRangeAsync(users);

            await context.SaveChangesAsync();
        }
    }
}
