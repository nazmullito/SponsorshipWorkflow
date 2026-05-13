using Sponsorship.Domain.Common;

namespace Sponsorship.Domain.Entities
{
    public class SponsorshipType : BaseEntity
    {
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
