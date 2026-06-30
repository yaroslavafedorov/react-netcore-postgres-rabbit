using Microsoft.EntityFrameworkCore;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using OrderService.Domain.Entities;

namespace OrderService.Persistence;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);

        // НАСТРОЙКА ТАБЛИЦ OUTBOX ДЛЯ MASSTRANSIT
        // Этот метод автоматически добавит в модель схемы таблиц: 
        // OutboxMessage, InboxState, OutboxState для EF Core
        modelBuilder.AddTransactionalOutboxEntities();
     }
}
