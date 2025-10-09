using Microsoft.EntityFrameworkCore;
using Alym.Shared.Models;

namespace Alym.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tariff> Tariffs { get; set; } = null!;
    }
}
