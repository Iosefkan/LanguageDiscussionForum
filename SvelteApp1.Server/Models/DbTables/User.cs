using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SvelteApp1.Server.Models
{
    public class User
    {
        [Required, Key]
        public string Guid { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public bool EmailConfirmed { get; set; }     
        [Required]
        public int RoleId { get; set; }
        [Required, ForeignKey("RoleId")]
        public Role Role { get; set; } = new();
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? AccessToken { get; set; }
        
    }
}
