using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SvelteApp1.Server.Models
{
    public class NewCommentInfo
    {
        [Required]
        public string? Comment { get; set; }
        public IFormFile? Audio { get; set; } = null;
        public string? Title { get; set; } = null;
    }
}
