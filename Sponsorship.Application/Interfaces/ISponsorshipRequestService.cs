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

        Task<SponsorshipRequest> SubmitAsync(Guid id);

        Task<SponsorshipRequest> CancelAsync(Guid id);

        Task<SponsorshipRequest> ManagerApproveAsync(Guid id, string? remarks);

        Task<SponsorshipRequest> ManagerRejectAsync(Guid id, string? remarks);

        Task<SponsorshipRequest> FinanceApproveAsync(Guid id, string? remarks);

        Task<SponsorshipRequest> FinanceRejectAsync(Guid id, string? remarks);

        Task<List<SponsorshipRequest>> GetMyRequestsAsync();

        Task<List<SponsorshipRequest>> GetPendingManagerRequestsAsync();

        Task<List<SponsorshipRequest>> GetPendingFinanceRequestsAsync();

        Task<List<ApprovalHistory>> GetHistoryAsync(Guid requestId);
        Task<List<SponsorshipRequest>> GetAllAsync();
    }
}
