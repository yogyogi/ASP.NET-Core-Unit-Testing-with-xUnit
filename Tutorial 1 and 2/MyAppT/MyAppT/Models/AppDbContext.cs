using Microsoft.EntityFrameworkCore;

namespace MyAppT.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Register> Register { get; set; }
    }
}
