namespace Sponsorship.Application.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalRequests { get; set; }

        public int PendingRequests { get; set; }

        public int ApprovedRequests { get; set; }

        public int RejectedRequests { get; set; }

        public int DraftRequests { get; set; }

        public int SponsorshipTypes { get; set; }

        public int WorkflowActions { get; set; }
    }
}
