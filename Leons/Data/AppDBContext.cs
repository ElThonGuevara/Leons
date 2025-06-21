using Leons.Models;
using Microsoft.EntityFrameworkCore;

namespace Leons.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        //DbSet<Cliente> Clientes { get; set; }
        //DbSet<Administrador> Administradores { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<DetalleCarrito> DetalleCarritos { get; set; }
        public DbSet<DetallePedido> DetallePedidos{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.rol)
                .WithMany(r => r.usuarios)
                .HasForeignKey(u => u.idRol)
                .OnDelete(DeleteBehavior.Restrict);
           // modelBuilder.Entity<Cliente>().HasOne(c => c.usuario)
           //     .WithMany(u => u.Clientes)
           //     .HasForeignKey(c => c.idUsuario)
           //     .OnDelete(DeleteBehavior.Restrict);
           //modelBuilder.Entity<Administrador>().HasOne(a => a.usuario)
           //     .WithMany(u => u.Administradores)
           //     .HasForeignKey(a => a.idUsuario)
           //     .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Pedido>().HasOne(p => p.usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.idUsuario)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.categoria)
                .WithMany(c => c.productos)
                .HasForeignKey(p => p.idCategoria)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Carrito>()
                .HasOne(c => c.usuario)
                .WithMany(u => u.Carritos)
                .HasForeignKey(c => c.idUsuario)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DetalleCarrito>()
                .HasOne(dc => dc.carrito)
                .WithMany(c => c.detalleCarrito)
                .HasForeignKey(dc => dc.idCarrito)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DetalleCarrito>().HasOne(dc => dc.Producto)
                .WithMany(p => p.detalleCarrito)
                .HasForeignKey(dc => dc.idProducto)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DetallePedido>().HasOne(dp => dp.pedido)
                .WithMany(p => p.detallePedidos)
                .HasForeignKey(dp => dp.idPedido)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DetallePedido>().HasOne(dp => dp.Producto).
                WithMany(p => p.detallePedidos)
                .HasForeignKey(dp => dp.idProducto)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Producto>(tb =>
            {
                tb.Property(col => col.precio).HasPrecision(10, 2);
            });
            modelBuilder.Entity<DetallePedido>(tb =>
            {
                tb.Property(col => col.precioUnitario).HasPrecision(10, 2);
            });
        }

        protected AppDBContext()
        {
        }
    }
}
