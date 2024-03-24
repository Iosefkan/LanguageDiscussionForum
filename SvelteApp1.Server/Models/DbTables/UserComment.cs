using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SvelteApp1.Server.Models
{
    public class UserComment
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public int CommentId { get; set; }
        [Required, ForeignKey("CommentId")]
        public Comment Comment { get; set; } = new();
        [Required]
        public string UserGuid { get; set; } = string.Empty;
        [Required, ForeignKey("UserGuid")]
        public User User { get; set; } = new();
        [Required]
        public bool Liked { get; set; }
        [Required]
        public bool Disliked { get; set; }
    }
}