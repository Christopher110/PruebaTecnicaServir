using Microsoft.EntityFrameworkCore;
using PruebaFullstack.Models;

namespace PruebaFullstack.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Proyecto> Proyectos => Set<Proyecto>();
        public DbSet<Rubro> Rubros => Set<Rubro>();
        public DbSet<Donacion> Donaciones => Set<Donacion>();
        public DbSet<OrdenCompra> OrdenesCompra => Set<OrdenCompra>();
        public DbSet<OrdenCompraDetalle> OrdenesCompraDetalle => Set<OrdenCompraDetalle>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rubro>()
                .HasIndex(r => new { r.ProyectoId, r.Nombre })
                .IsUnique();

            modelBuilder.Entity<OrdenCompraDetalle>()
                .HasOne(d => d.OrdenCompra)
                .WithMany(o => o.Detalles)
                .HasForeignKey(d => d.OrdenCompraId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrdenCompraDetalle>()
                .HasOne(d => d.Proyecto)
                .WithMany()
                .HasForeignKey(d => d.ProyectoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrdenCompraDetalle>()
                .HasOne(d => d.Rubro)
                .WithMany()
                .HasForeignKey(d => d.RubroId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
