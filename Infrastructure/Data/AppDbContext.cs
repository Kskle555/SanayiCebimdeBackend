using Microsoft.EntityFrameworkCore;
using SanayiCebimdeBackend.Domain.Entities;

namespace SanayiCebimdeBackend.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Galeri> Galeris => Set<Galeri>();
        public DbSet<Ustalar> Ustalar => Set<Ustalar>();
        public DbSet<Yorum> Yorums => Set<Yorum>();
        public DbSet<Skill> Skills => Set<Skill>();

        
    }
}