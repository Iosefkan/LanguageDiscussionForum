using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SvelteApp1.Server.Models
{
    public class Language
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string Lang { get; set; } = string.Empty;
    }
}
