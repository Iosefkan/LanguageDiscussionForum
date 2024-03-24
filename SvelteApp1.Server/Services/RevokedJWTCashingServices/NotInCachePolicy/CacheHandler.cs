using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using SvelteApp1.Server.Models;
using Microsoft.Net.Http.Headers;

namespace SvelteApp1.Server.Services
{
    public class CacheHandler : AuthorizationHandler<CacheRequirement>
    {
        readonly IMemoryCache cache;
        readonly IHttpContextAccessor httpContextAccessor;

        public CacheHandler(IMemoryCache cache, IHttpContextAccessor httpContextAccessor) 
        {
            this.cache = cache;
            this.httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CacheRequirement requirement)
        {
            // получаем claim с типом ClaimTypes.DateOfBirth - год рождения
            var accessToken = httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (accessToken is null)
            {
                context.Fail();
                Console.WriteLine("Fail one");
            }
                
            if (cache.TryGetValue(accessToken, out string _))
            {
                context.Fail();
                Console.WriteLine("Fail two");
            }
            else
            {
                context.Succeed(requirement);
                Console.WriteLine("Succ");
            }
            return Task.CompletedTask;
        }
    }
}
