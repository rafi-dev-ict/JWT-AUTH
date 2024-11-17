using Microsoft.EntityFrameworkCore;

namespace JWT_AUTH.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<LoginRequest> LoginRequest { get; set; }
        public DbSet<LoginResponse> LoginResponse { get; set; }
    }
}
