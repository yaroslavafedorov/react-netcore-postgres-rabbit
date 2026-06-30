using CatalogService.Services.Contracts.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Entities;
using OrderService.Persistence;

namespace OrderService.Services.Consumers;

/// <summary>
/// Потребитель события изменения цены товара из микросервиса каталога
/// </summary>
public class ProductPriceChangedConsumer(
    ILogger<ProductPriceChangedConsumer> logger,
    OrderDbContext dbContext) : IConsumer<ProductPriceChangedEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedEvent> context)
    {
        var message = context.Message;

        // Выводим красивый лог в консоль сервиса заказов
        logger.LogWarning("====================================================================");
        logger.LogWarning(" [RABBITMQ] ПРИНЯТО СОБЫТИЕ ИЗ КАТАЛОГА!");
        logger.LogWarning($" [RABBITMQ] Товар ID: {message.ProductId}");
        logger.LogWarning($" [RABBITMQ] Имя товара: {message.Name}");
        logger.LogWarning($" [RABBITMQ] Новая цена: {message.Price} руб.");
        
        // Магия MassTransit: уникальный системный ID сообщения
        logger.LogWarning($" [RABBITMQ] Системный MessageId: {context.MessageId}");
        logger.LogWarning("====================================================================");

        var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == message.ProductId);

        // Если товар не найден, то добавить запись
        if (product == null)
        {
            await dbContext.Products.AddAsync(
                new Product
                {
                    Id = message.ProductId,
                    Price = message.Price,
                    CreateDateTime = DateTimeOffset.UtcNow
                }
            );
        }
        else
        {
            product.Price = message.Price;  
            product.UpdateDateTime = DateTimeOffset.UtcNow;          
        }

        await dbContext.SaveChangesAsync();
    }
}
