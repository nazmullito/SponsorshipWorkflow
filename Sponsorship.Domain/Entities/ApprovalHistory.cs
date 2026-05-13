using Sponsorship.Domain.Common;
using Sponsorship.Domain.Enums;

namespace Sponsorship.Domain.Entities
{
    public class ApprovalHistory : BaseEntity
    {
        public Guid RequestId { get; set; }

        public SponsorshipRequest Request { get; set; } = default!;

        public string Action { get; set; } = default!;

        public SponsorshipRequestStatus FromStatus { get; set; }

        public SponsorshipRequestStatus ToStatus { get; set; }

        public string? Remarks { get; set; }

        public Guid PerformedByUserId { get; set; }

        public User PerformedByUser { get; set; } = default!;
    }
}
