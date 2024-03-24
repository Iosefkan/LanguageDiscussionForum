using System.ComponentModel.DataAnnotations;

namespace SvelteApp1.Server.Models
{
    public class LogginStatistic
    {
        [Required, Key]
        public int Id { get; set; }

        [Required] 
        public User? User { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
    }
}