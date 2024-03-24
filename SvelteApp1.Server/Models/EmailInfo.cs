using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SvelteApp1.Server.Models
{
    public class EmailInfo
    {
        [Required]
        public string? Email { get; set; }
    }
}