using Microsoft.EntityFrameworkCore;
using Sponsorship.Domain.Entities;

namespace Sponsorship.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<SponsorshipRequest> SponsorshipRequests => Set<SponsorshipRequest>();

        public DbSet<SponsorshipType> SponsorshipTypes => Set<SponsorshipType>();

        public DbSet<ApprovalHistory> ApprovalHistories => Set<ApprovalHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<SponsorshipRequest>()
                .Property(x => x.RequestedAmount)
                .HasPrecision(18, 2);
        }
    }
}
