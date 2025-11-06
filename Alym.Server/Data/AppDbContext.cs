using Alym.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Alym.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Все таблицы в одном контексте
        public DbSet<TariffCategory> TariffCategories { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<BusinessPreset> Presets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<BusinessPreset>()
        .HasKey(b => b.Id);
        
    modelBuilder.Entity<Question>()
        .HasKey(q => q.Id);
}
    }
}