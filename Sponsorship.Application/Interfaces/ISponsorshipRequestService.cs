using Sponsorship.Application.DTOs.Requests;
using Sponsorship.Domain.Entities;

namespace Sponsorship.Application.Interfaces
{
    public interface ISponsorshipRequestService
    {
        Task<SponsorshipRequest> CreateAsync(
            CreateSponsorshipRequestDto dto);

        Task<SponsorshipRequest> UpdateAsync(
            Guid id,
            UpdateSponsorshipRequestDto dto);

        Task SubmitAsync(Guid id);

        Task CancelAsync(Guid id);

        Task ManagerApproveAsync(Guid id, string? remarks);

        Task ManagerRejectAsync(Guid id, string? remarks);

        Task FinanceApproveAsync(Guid id, string? remarks);

        Task FinanceRejectAsync(Guid id, string? remarks);

        Task<List<SponsorshipRequest>> GetMyRequestsAsync();

        Task<List<SponsorshipRequest>> GetPendingManagerRequestsAsync();

        Task<List<SponsorshipRequest>> GetPendingFinanceRequestsAsync();

        Task<List<ApprovalHistory>> GetHistoryAsync(Guid requestId);
        Task<List<SponsorshipRequest>> GetAllAsync();
    }
}
