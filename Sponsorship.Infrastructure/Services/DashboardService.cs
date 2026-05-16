using Microsoft.EntityFrameworkCore;
using Sponsorship.Application.DTOs.Dashboard;
using Sponsorship.Application.Interfaces;
using Sponsorship.Domain.Enums;
using Sponsorship.Infrastructure.Persistence;

namespace Sponsorship.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDto>
            GetSummaryAsync()
        {
            return new DashboardSummaryDto
            {
                TotalRequests = await _context
                    .SponsorshipRequests
                    .CountAsync(),

                PendingRequests = await _context
                    .SponsorshipRequests
                    .CountAsync(x =>
                        x.Status == SponsorshipRequestStatus.PendingManagerApproval ||
                        x.Status == SponsorshipRequestStatus.PendingFinanceReview),

                ApprovedRequests = await _context
                    .SponsorshipRequests
                    .CountAsync(x =>
                        x.Status == SponsorshipRequestStatus.Approved),

                RejectedRequests = await _context
                    .SponsorshipRequests
                    .CountAsync(x =>
                        x.Status == SponsorshipRequestStatus.Rejected),

                DraftRequests = await _context
                    .SponsorshipRequests
                    .CountAsync(x =>
                        x.Status == SponsorshipRequestStatus.Draft),

                SponsorshipTypes = await _context
                    .SponsorshipTypes
                    .CountAsync(),

                WorkflowActions = await _context
                    .ApprovalHistories
                    .CountAsync()
            };
        }
    }
}
