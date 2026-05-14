using Microsoft.AspNetCore.Http;
using Sponsorship.Application.Interfaces;
using System.Security.Claims;

namespace Sponsorship.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId =>
            Guid.Parse(
                _httpContextAccessor.HttpContext?
                    .User
                    .FindFirstValue(ClaimTypes.NameIdentifier)
                ??
                throw new Exception("User ID not found"));

        public string Email =>
            _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.Email)
            ?? string.Empty;

        public string Role =>
            _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.Role)
            ?? string.Empty;

        public string Name =>
            _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.Name)
            ?? string.Empty;
    }
}
