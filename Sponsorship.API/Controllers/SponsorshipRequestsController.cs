using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sponsorship.Application.Common;
using Sponsorship.Application.DTOs.Requests;
using Sponsorship.Application.Interfaces;
using Sponsorship.Domain.Entities;

namespace Sponsorship.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SponsorshipRequestsController : ControllerBase
    {
        private readonly ISponsorshipRequestService _service;

        public SponsorshipRequestsController(ISponsorshipRequestService service)
        {
            _service = service;
        }

        [HttpGet("my")]
        [Authorize(Roles = "Requestor")]
        public async Task<IActionResult> GetMyRequests()
        {
            var result = await _service.GetMyRequestsAsync();

            return Ok(ApiResponse<List<SponsorshipRequest>>.SuccessResponse(result, "Requests retrieved successfully"));
        }

        [HttpGet("pending-manager")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetPendingManager()
        {
            var result = await _service.GetPendingManagerRequestsAsync();

            return Ok(ApiResponse<List<SponsorshipRequest>>.SuccessResponse(result, "Pending manager requests retrieved successfully"));
        }

        [HttpGet("pending-finance")]
        [Authorize(Roles = "FinanceAdmin")]
        public async Task<IActionResult> GetPendingFinance()
        {
            var result = await _service.GetPendingFinanceRequestsAsync();

            return Ok(ApiResponse<List<SponsorshipRequest>>.SuccessResponse(result, "Pending finance requests retrieved successfully"));
        }

        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetHistory(Guid id)
        {
            var result = await _service.GetHistoryAsync(id);

            return Ok(ApiResponse<List<ApprovalHistory>>.SuccessResponse(result, "Workflow history retrieved successfully"));
        }

        [HttpPost]
        [Authorize(Roles = "Requestor")]
        public async Task<IActionResult> Create(CreateSponsorshipRequestDto dto)
        {
            var result = await _service.CreateAsync(dto);

            return Ok(ApiResponse<SponsorshipRequest>.SuccessResponse(result, "Request created successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Requestor")]
        public async Task<IActionResult> Update(Guid id, UpdateSponsorshipRequestDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            return Ok(ApiResponse<SponsorshipRequest>.SuccessResponse(result, "Request updated successfully"));
        }

        [HttpPost("{id}/submit")]
        [Authorize(Roles = "Requestor")]
        public async Task<IActionResult> Submit(Guid id)
        {
            await _service.SubmitAsync(id);

            return Ok(ApiResponse<object>.SuccessResponse(null, "Request submitted successfully"));
        }

        [HttpPost("{id}/cancel")]
        [Authorize(Roles = "Requestor")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _service.CancelAsync(id);

            return Ok(ApiResponse<object>.SuccessResponse(null, "Request cancelled successfully"));
        }

        [HttpPost("{id}/manager-approve")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ManagerApprove(Guid id, ApprovalActionDto dto)
        {
            await _service.ManagerApproveAsync(id, dto.Remarks);

            return Ok(ApiResponse<object>.SuccessResponse(null, "Request approved by manager"));
        }

        [HttpPost("{id}/manager-reject")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ManagerReject(Guid id, ApprovalActionDto dto)
        {
            await _service.ManagerRejectAsync(id, dto.Remarks);

            return Ok(ApiResponse<object>.SuccessResponse(null, "Request rejected by manager"));
        }

        [HttpPost("{id}/finance-approve")]
        [Authorize(Roles = "FinanceAdmin")]
        public async Task<IActionResult> FinanceApprove(Guid id, ApprovalActionDto dto)
        {
            await _service.FinanceApproveAsync(id, dto.Remarks);

            return Ok(ApiResponse<object>.SuccessResponse(null, "Request approved by finance"));
        }

        [HttpPost("{id}/finance-reject")]
        [Authorize(Roles = "FinanceAdmin")]
        public async Task<IActionResult> FinanceReject(Guid id, ApprovalActionDto dto)
        {
            await _service.FinanceRejectAsync(id, dto.Remarks);

            return Ok(ApiResponse<object>.SuccessResponse(null, "Request rejected by finance"));
        }
    }
}
