using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sponsorship.Application.Common;
using Sponsorship.Application.DTOs.Dashboard;
using Sponsorship.Application.Interfaces;

namespace Sponsorship.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _service.GetSummaryAsync();

            return Ok(ApiResponse<DashboardSummaryDto>.SuccessResponse(result, "Dashboard summary retrieved successfully"));
        }
    }
}
