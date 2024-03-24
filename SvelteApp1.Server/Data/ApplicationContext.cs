using HashLibrary;
using Microsoft.EntityFrameworkCore;
using SvelteApp1.Server.Models;

namespace SvelteApp1.Server.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Language> Languages { get; set; } = null!;
        public DbSet<LogginStatistic> logginStatistics { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<UserComment> UserComments { get; set; } = null!;
        public DbSet<UserQuestion> UserQuestions { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
