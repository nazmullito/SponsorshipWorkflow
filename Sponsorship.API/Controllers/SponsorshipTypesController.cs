using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sponsorship.Application.Common;
using Sponsorship.Domain.Entities;
using Sponsorship.Infrastructure.Persistence;

namespace Sponsorship.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "SystemAdmin")]
    public class SponsorshipTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SponsorshipTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<SponsorshipType>>>> GetAll()
        {
            var data = await _context.SponsorshipTypes
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Ok(ApiResponse<List<SponsorshipType>>.SuccessResponse(data, "Sponsorship types retrieved"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<SponsorshipType>>> Create(SponsorshipType request)
        {
            request.Id = Guid.NewGuid();

            _context.SponsorshipTypes.Add(request);

            await _context.SaveChangesAsync();

            return Ok(ApiResponse<SponsorshipType>.SuccessResponse(request, "Sponsorship type created"));
        }
    }
}
