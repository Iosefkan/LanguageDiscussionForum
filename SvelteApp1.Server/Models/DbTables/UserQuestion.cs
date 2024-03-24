using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SvelteApp1.Server.Models
{
    public class UserQuestion
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required, ForeignKey("QuestionId")]
        public Question Question { get; set; } = new();
        [Required]
        public string UserGuid { get; set; } = string.Empty;
        [Required, ForeignKey("UserGuid")]
        public User User { get; set; } = new();
    }
}