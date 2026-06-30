using Microsoft.EntityFrameworkCore;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using OrderService.Domain.Entities;

namespace OrderService.Persistence;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Order>().Property(o => o.Total).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<OrderItem>().Property(o => o.Price).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Order>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<OrderItem>().HasQueryFilter(p => !p.IsDeleted);

        // НАСТРОЙКА ТАБЛИЦ OUTBOX ДЛЯ MASSTRANSIT
        // Этот метод автоматически добавит в модель схемы таблиц: 
        // OutboxMessage, InboxState, OutboxState для EF Core
        modelBuilder.AddTransactionalOutboxEntities();
     }
}
