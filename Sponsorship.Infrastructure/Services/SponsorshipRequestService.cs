using Microsoft.EntityFrameworkCore;
using Sponsorship.Application.Common.Exceptions;
using Sponsorship.Application.DTOs.Requests;
using Sponsorship.Application.Interfaces;
using Sponsorship.Domain.Entities;
using Sponsorship.Domain.Enums;
using Sponsorship.Infrastructure.Persistence;

namespace Sponsorship.Infrastructure.Services
{
    public class SponsorshipRequestService : ISponsorshipRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public SponsorshipRequestService(ApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<SponsorshipRequest> CreateAsync(CreateSponsorshipRequestDto dto)
        {
            var entity = new SponsorshipRequest
            {
                Title = dto.Title,
                RequestorName = dto.RequestorName,
                Department = dto.Department,
                SponsorshipTypeId = dto.SponsorshipTypeId,
                EventName = dto.EventName,
                EventDate = DateTime.SpecifyKind(dto.EventDate, DateTimeKind.Utc),
                RequestedAmount = dto.RequestedAmount,
                Purpose = dto.Purpose,
                ExpectedBusinessBenefit = dto.ExpectedBusinessBenefit,
                Remarks = dto.Remarks,
                CreatedByUserId = _currentUser.UserId,
                Status = SponsorshipRequestStatus.Draft
            };

            _context.SponsorshipRequests.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<SponsorshipRequest> UpdateAsync(Guid id, UpdateSponsorshipRequestDto dto)
        {
            var entity = await GetOwnedRequest(id);

            if (entity.Status != SponsorshipRequestStatus.Draft)
            {
                throw new BadRequestException(
                    "Only draft requests can be edited");
            }

            entity.Title = dto.Title;
            entity.Department = dto.Department;
            entity.SponsorshipTypeId = dto.SponsorshipTypeId;
            entity.EventName = dto.EventName;
            entity.EventDate = dto.EventDate;
            entity.RequestedAmount = dto.RequestedAmount;
            entity.Purpose = dto.Purpose;
            entity.ExpectedBusinessBenefit =
                dto.ExpectedBusinessBenefit;
            entity.Remarks = dto.Remarks;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<SponsorshipRequest> SubmitAsync(Guid id)
        {
            var entity = await GetOwnedRequest(id);

            ValidateTransition(entity.Status, SponsorshipRequestStatus.PendingManagerApproval);

            var oldStatus = entity.Status;

            entity.Status =
                SponsorshipRequestStatus.PendingManagerApproval;

            await AddHistory(entity.Id, "Submitted", oldStatus, entity.Status);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<SponsorshipRequest> CancelAsync(Guid id)
        {
            var entity = await GetOwnedRequest(id);

            if (entity.Status == SponsorshipRequestStatus.Approved)
            {
                throw new BadRequestException(
                    "Approved request cannot be cancelled");
            }

            var oldStatus = entity.Status;

            entity.Status = SponsorshipRequestStatus.Cancelled;

            await AddHistory(entity.Id, "Cancelled", oldStatus, entity.Status);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<SponsorshipRequest> ManagerApproveAsync(Guid id, string? remarks)
        {
            EnsureRole("Manager");

            var entity = await GetRequest(id);

            ValidateTransition(entity.Status, SponsorshipRequestStatus.PendingFinanceReview);

            var oldStatus = entity.Status;

            entity.Status = SponsorshipRequestStatus.PendingFinanceReview;

            await AddHistory(entity.Id, "Manager Approved", oldStatus, entity.Status, remarks);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<SponsorshipRequest> ManagerRejectAsync(Guid id, string? remarks)
        {
            EnsureRole("Manager");

            var entity = await GetRequest(id);

            var oldStatus = entity.Status;

            entity.Status = SponsorshipRequestStatus.Rejected;

            await AddHistory(entity.Id, "Manager Rejected", oldStatus, entity.Status, remarks);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<SponsorshipRequest> FinanceApproveAsync(Guid id, string? remarks)
        {
            EnsureRole("FinanceAdmin");

            var entity = await GetRequest(id);

            ValidateTransition(entity.Status, SponsorshipRequestStatus.Approved);

            var oldStatus = entity.Status;

            entity.Status = SponsorshipRequestStatus.Approved;

            await AddHistory(entity.Id, "Finance Approved", oldStatus, entity.Status, remarks);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<SponsorshipRequest> FinanceRejectAsync(Guid id, string? remarks)
        {
            EnsureRole("FinanceAdmin");

            var entity = await GetRequest(id);

            var oldStatus = entity.Status;

            entity.Status = SponsorshipRequestStatus.Rejected;

            await AddHistory(entity.Id, "Finance Rejected", oldStatus, entity.Status, remarks);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<SponsorshipRequest>> GetMyRequestsAsync()
        {
            return await _context.SponsorshipRequests
                .Where(x => x.CreatedByUserId == _currentUser.UserId)
                .ToListAsync();
        }

        public async Task<List<SponsorshipRequest>> GetPendingManagerRequestsAsync()
        {
            EnsureRole("Manager");

            return await _context.SponsorshipRequests
                .Where(x => x.Status == SponsorshipRequestStatus.PendingManagerApproval)
                .ToListAsync();
        }

        public async Task<List<SponsorshipRequest>> GetPendingFinanceRequestsAsync()
        {
            EnsureRole("FinanceAdmin");

            return await _context.SponsorshipRequests
                .Where(x => x.Status == SponsorshipRequestStatus.PendingFinanceReview)
                .ToListAsync();
        }

        public async Task<List<ApprovalHistory>> GetHistoryAsync(Guid requestId)
        {
            return await _context.ApprovalHistories
                .Where(x => x.RequestId == requestId)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }

        private async Task<SponsorshipRequest> GetRequest(Guid id)
        {
            var entity = await _context.SponsorshipRequests
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Request not found");
            }

            return entity;
        }

        private async Task<SponsorshipRequest> GetOwnedRequest(Guid id)
        {
            var entity = await _context.SponsorshipRequests
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CreatedByUserId == _currentUser.UserId);

            if (entity is null)
            {
                throw new NotFoundException("Request not found");
            }

            return entity;
        }

        private void ValidateTransition(SponsorshipRequestStatus current, SponsorshipRequestStatus target)
        {
            var valid = current switch
            {
                SponsorshipRequestStatus.Draft
                    when target ==
                        SponsorshipRequestStatus
                            .PendingManagerApproval
                    => true,

                SponsorshipRequestStatus.PendingManagerApproval
                    when target ==
                        SponsorshipRequestStatus
                            .PendingFinanceReview
                    => true,

                SponsorshipRequestStatus.PendingFinanceReview
                    when target ==
                        SponsorshipRequestStatus.Approved
                    => true,

                _ => false
            };

            if (!valid)
            {
                throw new BadRequestException(
                    $"Invalid workflow transition from {current} to {target}");
            }
        }

        private void EnsureRole(string role)
        {
            if (_currentUser.Role != role)
            {
                throw new ForbiddenException(
                    "Access denied");
            }
        }

        private async Task AddHistory(
            Guid requestId,
            string action,
            SponsorshipRequestStatus fromStatus,
            SponsorshipRequestStatus toStatus,
            string? remarks = null)
        {
            var history = new ApprovalHistory
            {
                RequestId = requestId,
                Action = action,
                FromStatus = fromStatus,
                ToStatus = toStatus,
                Remarks = remarks,
                PerformedByUserId = _currentUser.UserId
            };

            _context.ApprovalHistories.Add(history);

            await Task.CompletedTask;
        }

        public async Task<List<SponsorshipRequest>> GetAllAsync()
        {
            return await _context.SponsorshipRequests
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
