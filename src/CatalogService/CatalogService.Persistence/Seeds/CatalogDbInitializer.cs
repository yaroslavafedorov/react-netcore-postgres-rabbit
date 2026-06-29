using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CatalogService.Persistence.Seeds;

public static class CatalogDbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        
        var context = services.GetRequiredService<CatalogDbContext>();
        var configuration = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<CatalogDbContext>>();

        // 1. Проверяем главный флаг
        var shouldSeed = configuration.GetValue<bool>("DatabaseSettings:SeedDatabase");
        if (!shouldSeed)
        {
            logger.LogInformation("Сидинг тестовых данных выключен в конфигурации.");
            return;
        }

        // 2. Инициализируем принудительный сброс (полезно для разработки при обновлении моделей)
        var forceRecreate = configuration.GetValue<bool>("DatabaseSettings:ForceRecreateData");
        if (forceRecreate)
        {
            logger.LogWarning("ForceRecreateData включена. Очистка существующих таблиц Каталога...");
            
            // Очищаем продукты, затем категории (соблюдая зависимости внешних ключей)
            context.Products.RemoveRange(context.Products);
            context.Categories.RemoveRange(context.Categories);
            await context.SaveChangesAsync();
            
            logger.LogInformation("БД успешно очищена.");
        }
        else
        {
            // Если сброс отключен, работаем по старой схеме: проверяем наличие данных
            if (await context.Categories.AnyAsync() || await context.Products.AnyAsync())
            {
                logger.LogInformation("База данных содержит данные. Сидирование тестовых данных пропущено.");
                return;
            }
        }

        logger.LogInformation("Старт сидирования новых тестовых данных...");

        // Идентификаторы для жесткой связи
        var electronicsId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var booksId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        // Создаем категории 
        var categories = new List<Category>
        {
            new() { Id = electronicsId, Name = "Электроника", Description = "Гатжеты, смартфоны и девайсы", CreateDateTime = DateTimeOffset.UtcNow },
            new() { Id = booksId, Name = "Книги", Description = "Техническая и художественная литература", CreateDateTime = DateTimeOffset.UtcNow }
        };

        // Создаем продукты
        var products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "Смартфон", Description = "Флагманский телефон с отличной камерой", Price = 50000, CategoryId = electronicsId, CreateDateTime = DateTimeOffset.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Ноутбук", Description = "Мощный ноутбук для разработки на .NET и React", Price = 90000, CategoryId = electronicsId, CreateDateTime = DateTimeOffset.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Чистый Код", Description = "Легендарная книга Роберта Мартина", Price = 1500, CategoryId = booksId, CreateDateTime = DateTimeOffset.UtcNow }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        logger.LogInformation("Сидирование тестовых данных успешно завершено!");
    }
}
