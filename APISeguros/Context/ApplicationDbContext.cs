using APISeguros.Models;
using Microsoft.EntityFrameworkCore;

namespace APISeguros.Context
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        { 

        }

        public DbSet<Seguro> Seguros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seguro>().ToTable("Seguro");

            modelBuilder.Entity<Seguro>()
            .HasIndex(s => s.NumeroIdentificacion)
            .IsUnique();
        }
    }
}
