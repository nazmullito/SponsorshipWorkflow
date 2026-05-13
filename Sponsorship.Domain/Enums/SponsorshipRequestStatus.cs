namespace Sponsorship.Domain.Enums
{
    public enum SponsorshipRequestStatus
    {
        Draft = 1,
        PendingManagerApproval = 2,
        PendingFinanceReview = 3,
        Approved = 4,
        Rejected = 5,
        Cancelled = 6
    }
}
