using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Persistence;
using OrderService.Services.Contracts.Dtos;
using OrderService.Services.Contracts.Interfaces;

namespace OrderService.Services.Commands;

/// <inheritdoc/>
public class OrderCommands(OrderDbContext dbContext) : IOrderCommands
{
    /// <inheritdoc/>
    public async Task<Guid> CreateOrderAsync(CreateOrderRequestDto order, CancellationToken cancellationToken)
    {
        if (order.Items.Count == 0)
        {
            throw new Exception("Заказ должен содержать как минимум одну позицию.");
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        {
            try
            {
                var productIds = order.Items.Select(x => x.ProductId).Distinct();
                var prices = await dbContext.Products
                    .AsNoTracking()
                    .Where(x => productIds.Contains(x.Id))
                    .ToDictionaryAsync(x => x.Id, x => x.Price, cancellationToken);

                var newOrder = new Order
                {
                    Id = Guid.NewGuid(),
                    Status = "Created",
                    OrderItems = []
                };

                foreach (var item in order.Items)
                {
                    if (!prices.TryGetValue(item.ProductId, out var price))
                    {
                        throw new Exception($"Цена товара с ИД: {item.ProductId} не найдена.");
                    }

                    newOrder.Total += price * item.Quantity;
                    newOrder.OrderItems.Add(new OrderItem
                    {
                        OrderId = newOrder.Id,
                        ProductId = item.ProductId,
                        Price = price,
                        Quantity = item.Quantity,
                        CreateDateTime = DateTimeOffset.UtcNow
                    });
                }

                newOrder.CreateDateTime = DateTimeOffset.UtcNow;

                await dbContext.Orders.AddAsync(newOrder, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return newOrder.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new Exception("Ошибка создания заказа", ex);
            }
        }
    }
}
