using System.Security.Claims;

namespace SvelteApp1.Server
{
    public static class JwtConfig
    {
        public static string ConfigIssuer = "JwtSettings:Issuer";
        public static string ConfigAudience = "JwtSettings:Audience";
        public static string ConfigKey = "JwtSettings:Key";
        public static string ClaimGuid = "guid";
        public static TimeSpan AccessTokenExpiration = TimeSpan.FromMinutes(15);
        public static TimeSpan RefreshTokenExpiration = TimeSpan.FromDays(3);
        public static class Polices
        {
            public const string NotLoggedOut = "NotRevoked";
        }
    }
}
