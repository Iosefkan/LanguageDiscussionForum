using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace SvelteApp1.Server.Models
{
    public class RegUserInfo
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
