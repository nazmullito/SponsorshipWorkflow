using Sponsorship.Application.DTOs.Dashboard;

namespace Sponsorship.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
    }
}
