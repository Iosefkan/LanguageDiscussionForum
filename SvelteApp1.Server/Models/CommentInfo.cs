using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SvelteApp1.Server.Models
{
    public class CommentInfo
    {
        [Required]
        public int CommentId { get; set; }
        [Required]
        public string? Comment { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public int Downvotes { get; set; }
        [Required]
        public int Upvotes { get; set; }
        [Required]
        public bool Liked { get; set; }
        [Required]
        public bool Disliked { get; set; }
        public string? FileName { get; set; } = null; 
        public string? FileTitle { get; set; } = null;

    }
}