using System.ComponentModel.DataAnnotations;

namespace SvelteApp1.Server.Models
{
    public class LoginData
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
