using Microsoft.Extensions.Caching.Memory;
using SvelteApp1.Server.Models;

namespace SvelteApp1.Server.Services
{
    public class RevokedJWTCashingService
    {
        readonly IMemoryCache cache;
        public RevokedJWTCashingService(IMemoryCache cache)
        {
            this.cache = cache;
        }
        public void RevokeJWT(string accessToken, string refreshToken)
        {
            Console.WriteLine("Acc tok revoked");
            Console.WriteLine(accessToken);
            cache.Set(accessToken, refreshToken, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
            if (cache.TryGetValue(accessToken, out var token))
            {
                Console.WriteLine("Ref revoked in cache");
                Console.WriteLine(token);
            }
        }
    }
}
