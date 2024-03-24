using Microsoft.AspNetCore.Authorization;

namespace SvelteApp1.Server.Services
{
    public class CacheRequirement : IAuthorizationRequirement
    {
        public CacheRequirement() { }
    }
}
