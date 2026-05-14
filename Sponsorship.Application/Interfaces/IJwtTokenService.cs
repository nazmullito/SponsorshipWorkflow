using Sponsorship.Domain.Entities;

namespace Sponsorship.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
