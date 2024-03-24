using SvelteApp1.Server.Models;
using System.Security.Claims;

namespace SvelteApp1.Server.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(TimeSpan expiration, User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
