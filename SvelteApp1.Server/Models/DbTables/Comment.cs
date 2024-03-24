using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SvelteApp1.Server.Models
{
    public class Comment
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string Comm { get; set; } = string.Empty;
        [Required]
        public string UserGuid { get; set; } = string.Empty;
        [Required, ForeignKey("UserGuid")]
        public User User { get; set; } = new User();
        [Required]
        public int QuestionId { get; set; }
        [Required, ForeignKey("QuestionId")]
        public Question Question { get; set; } = new();
        [Required]
        public int Likes { get; set; } = 0;
        [Required]
        public int Dislikes { get; set; } = 0;
        public string? FileName { get; set; } = null;
        public string? FileTitle { get; set; } = null;
    }
}
