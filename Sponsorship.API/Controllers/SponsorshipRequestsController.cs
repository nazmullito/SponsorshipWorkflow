using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sponsorship.Application.DTOs.Requests;
using Sponsorship.Application.Interfaces;

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
        public async Task<IActionResult> GetMyRequests()
        {
            return Ok(await _service.GetMyRequestsAsync());
        }

        [HttpGet("pending-manager")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetPendingManager()
        {
            return Ok(await _service.GetPendingManagerRequestsAsync());
        }

        [HttpGet("pending-finance")]
        [Authorize(Roles = "FinanceAdmin")]
        public async Task<IActionResult> GetPendingFinance()
        {
            return Ok(await _service.GetPendingFinanceRequestsAsync());
        }

        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetHistory(Guid id)
        {
            return Ok(await _service.GetHistoryAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Requestor")]
        public async Task<IActionResult> Create(CreateSponsorshipRequestDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Requestor")]
        public async Task<IActionResult> Update(Guid id, UpdateSponsorshipRequestDto dto)
        {
            return Ok(await _service.UpdateAsync(id, dto));
        }

        [HttpPost("{id}/submit")]
        [Authorize(Roles = "Requestor")]
        public async Task<IActionResult> Submit(Guid id)
        {
            await _service.SubmitAsync(id);

            return Ok();
        }

        [HttpPost("{id}/cancel")]
        [Authorize(Roles = "Requestor")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _service.CancelAsync(id);

            return Ok();
        }

        [HttpPost("{id}/manager-approve")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ManagerApprove(Guid id, ApprovalActionDto dto)
        {
            await _service.ManagerApproveAsync(id, dto.Remarks);

            return Ok();
        }

        [HttpPost("{id}/manager-reject")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ManagerReject(Guid id, ApprovalActionDto dto)
        {
            await _service.ManagerRejectAsync(id, dto.Remarks);

            return Ok();
        }

        [HttpPost("{id}/finance-approve")]
        [Authorize(Roles = "FinanceAdmin")]
        public async Task<IActionResult> FinanceApprove(Guid id, ApprovalActionDto dto)
        {
            await _service.FinanceApproveAsync(id, dto.Remarks);

            return Ok();
        }

        [HttpPost("{id}/finance-reject")]
        [Authorize(Roles = "FinanceAdmin")]
        public async Task<IActionResult> FinanceReject(Guid id, ApprovalActionDto dto)
        {
            await _service.FinanceRejectAsync(id, dto.Remarks);

            return Ok();
        }
    }
}
