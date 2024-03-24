using System.ComponentModel.DataAnnotations;

namespace SvelteApp1.Server.Models
{
    public class NewQuestionInfo
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Question { get; set; }
        [Required]
        public string? Language { get; set; }
    }
}
