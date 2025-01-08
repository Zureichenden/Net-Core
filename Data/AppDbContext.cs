using Microsoft.EntityFrameworkCore;
using BackendApi.Models; 

namespace BackendApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet para la tabla 'users'
        public DbSet<User> Users { get; set; }
    }
}
