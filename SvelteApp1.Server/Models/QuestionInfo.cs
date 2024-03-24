using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SvelteApp1.Server.Models
{
    public class QuestionInfo
    {
        [Required]
        public string Language { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string Question { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public int ViewCount { get; set; }
        [Required]
        public int Upvotes { get; set; }
        [Required]
        public bool Liked { get; set; } = false;

    }
}