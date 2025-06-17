using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Models;

namespace TaskManagerApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TaskManagerApp;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        
    }
}
