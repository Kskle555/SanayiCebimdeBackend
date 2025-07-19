using Microsoft.EntityFrameworkCore;
using SanayiCebimdeBackend.Domain.Entities;

namespace SanayiCebimdeBackend.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        // Add other DbSets
    }
}