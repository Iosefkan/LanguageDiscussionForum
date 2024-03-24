using Microsoft.IdentityModel.Tokens;
using SvelteApp1.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SvelteApp1.Server.Services
{
    public class TokenService : ITokenService
    {
        IConfiguration config;
        public TokenService(IConfiguration configuration) 
        {
            this.config = configuration;
        }
        public string GenerateAccessToken(TimeSpan expiration, User user)
        {
            var claims = GetUserClaimsAndJti(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[JwtConfig.ConfigKey]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                issuer: config[JwtConfig.ConfigIssuer],
                audience: config[JwtConfig.ConfigAudience],
                expires: DateTime.UtcNow.Add(expiration),
                claims: claims,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config[JwtConfig.ConfigIssuer],
                ValidateAudience = true,
                ValidAudience = config[JwtConfig.ConfigAudience],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[JwtConfig.ConfigKey]!)),
                ValidateIssuerSigningKey = true,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
        private static List<Claim> GetUserClaimsAndJti(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtConfig.ClaimGuid, user.Guid),
            };
            return claims;
        }
    }
}
