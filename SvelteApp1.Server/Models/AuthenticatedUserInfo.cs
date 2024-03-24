using System.ComponentModel.DataAnnotations;

namespace SvelteApp1.Server.Models
{
    public class AuthenticatedUserInfo
    {
        [Required]
        public string? AccessToken { get; set; }
        [Required]
        public string? RefreshToken { get; set; }
    }
}
