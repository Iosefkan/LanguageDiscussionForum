using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SvelteApp1.Server.Models
{
    public class UserInfo
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; }
        public int Upvotes { get; set; }
        public int CommentCount { get; set; }
        public int QuestionCount { get; set; }
    }
}