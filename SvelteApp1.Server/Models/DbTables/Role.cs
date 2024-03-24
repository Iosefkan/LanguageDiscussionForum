using System.ComponentModel.DataAnnotations;

namespace SvelteApp1.Server.Models
{
    public class Role
    {
        [Required, Key]
        public int Id { get; set; }

        [Required] 
        public string Name { get; set; } = string.Empty;
    }
}
