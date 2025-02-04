using Microsoft.EntityFrameworkCore;


using AbbouClima.Models;

namespace AbbouClima.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Presupuesto> Presupuestos { get; set; }
        
    }
}
