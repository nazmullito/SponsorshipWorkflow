using Sponsorship.Domain.Common;
using Sponsorship.Domain.Enums;

namespace Sponsorship.Domain.Entities
{
    public class SponsorshipRequest : BaseEntity
    {
        public string Title { get; set; } = default!;

        public string RequestorName { get; set; } = default!;

        public string Department { get; set; } = default!;

        public Guid SponsorshipTypeId { get; set; }

        public SponsorshipType SponsorshipType { get; set; } = default!;

        public string EventName { get; set; } = default!;

        public DateTime EventDate { get; set; }

        public decimal RequestedAmount { get; set; }

        public string Purpose { get; set; } = default!;

        public string? ExpectedBusinessBenefit { get; set; }

        public string? Remarks { get; set; }

        public SponsorshipRequestStatus Status { get; set; }
            = SponsorshipRequestStatus.Draft;

        public Guid CreatedByUserId { get; set; }

        public User CreatedByUser { get; set; } = default!;
    }
}
