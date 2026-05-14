using Sponsorship.Application.Interfaces;
using Sponsorship.Domain.Entities;
using Sponsorship.Domain.Enums;

namespace Sponsorship.Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, IPasswordHasher passwordHasher)
        {
            // Seed Sponsorship Types
            if (!context.SponsorshipTypes.Any())
            {
                var sponsorshipTypes = new List<SponsorshipType>
                {
                    new()
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Conference",
                        Description = "Conference Sponsorship"
                    },

                    new()
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Name = "Community Event",
                        Description = "Community Event Sponsorship"
                    },

                    new()
                    {
                        Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        Name = "Education",
                        Description = "Education Sponsorship"
                    }
                };

                await context.SponsorshipTypes.AddRangeAsync(
                    sponsorshipTypes);

                await context.SaveChangesAsync();
            }

            // Seed Users
            if (!context.Users.Any())
            {
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
}