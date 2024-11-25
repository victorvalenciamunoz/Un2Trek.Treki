using ErrorOr;
using System.Security.Claims;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public interface IJwtService
{
    string GenerateRefreshToken();
    ErrorOr<string> GenerateToken(ApplicationUser user);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}