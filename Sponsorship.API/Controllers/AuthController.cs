using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sponsorship.Application.Common;
using Sponsorship.Application.DTOs.Auth;
using Sponsorship.Application.Interfaces;
using Sponsorship.Infrastructure.Persistence;

namespace Sponsorship.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthController(
            ApplicationDbContext context,
            IJwtTokenService jwtTokenService,
            IPasswordHasher passwordHasher)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user is null)
            {
                return Unauthorized(ApiResponse<object>.FailureResponse("Invalid credentials"));
            }

            var validPassword = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!validPassword)
            {
                return Unauthorized(ApiResponse<object>.FailureResponse("Invalid credentials"));
            }

            var token = _jwtTokenService.GenerateToken(user);

            var response = new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role.ToString(),
                Name = user.Name
            };

            return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login successful"));
        }
    }
}
