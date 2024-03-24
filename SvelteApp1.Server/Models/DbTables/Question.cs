using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SvelteApp1.Server.Models
{
    public class Question
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Quest { get; set; } = string.Empty;
        [Required]
        public Language? Language { get; set; }
        [Required]
        public string UserGuid { get; set; } = string.Empty;
        [Required, ForeignKey("UserGuid")]
        public User User { get; set; } = new User();
        [Required]
        public int ViewCount { get; set; }
        [Required]
        public int Upvotes { get; set; } = 0;
        public List<Comment> Comments { get; set; } = new();


    }
}
