using backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class AppDbContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
    }
}
