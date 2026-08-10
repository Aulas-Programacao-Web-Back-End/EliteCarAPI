using EliteCarAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EliteCarAPI.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Carro> Carros { get; set; }
    public DbSet<PedidoVenda> PedidosVenda { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // RN01.1 — CPF único por cliente
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Cpf)
            .IsUnique()
            .HasDatabaseName("IX_clientes_cpf");

        // RN01.2 — E-mail único por cliente
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("IX_clientes_email");

        // RN06.1 — Placa única por veículo
        modelBuilder.Entity<Carro>()
            .HasIndex(c => c.Placa)
            .IsUnique()
            .HasDatabaseName("IX_carros_placa");

        // Precisão de decimais
        modelBuilder.Entity<Carro>()
            .Property(c => c.Preco)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Carro>()
            .Property(c => c.Quilometragem)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PedidoVenda>()
            .Property(p => p.ValorPedido)
            .HasPrecision(18, 2);

        // Relacionamentos
        modelBuilder.Entity<PedidoVenda>()
            .HasOne(p => p.Cliente)
            .WithMany(c => c.PedidosVenda)
            .HasForeignKey(p => p.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PedidoVenda>()
            .HasOne(p => p.Carro)
            .WithMany(c => c.PedidosVenda)
            .HasForeignKey(p => p.IdCarro)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
