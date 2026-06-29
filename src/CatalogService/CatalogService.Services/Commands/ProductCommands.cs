using CatalogService.Domain.Entities;
using CatalogService.Persistence;
using CatalogService.Services.Contracts.Dtos;
using CatalogService.Services.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Services.Commands;

public class ProductCommands(CatalogDbContext dbContext) : IProductCommands
{
    /// <inheritdoc/>
    public async Task<Guid> AddProductAsync(CreateProductRequestDto newProduct, CancellationToken cancellationToken)
    {
        var entity = new Product
        {
            Id = Guid.NewGuid(),
            Name = newProduct.Name,
            Description = newProduct.Description,
            Price = newProduct.Price,
            CategoryId = newProduct.CategoryId,

            CreateDateTime = DateTimeOffset.UtcNow,
            UpdateDateTime = DateTimeOffset.UtcNow
        };

        await dbContext.Products.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    /// <inheritdoc/>
    public async Task<Guid> UpdateProductsync(Guid id, UpdateProductRequestDto product, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken) 
            ?? throw new Exception($"Товар с ИД:{id} не найден.");

        entity.Name = product.Name;
        entity.Description = product.Description;
        entity.Price = product.Price;
        entity.UpdateDateTime = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        //TODO добавить отправку сообщения в МассТранзит с исользованием транзакшен аутбокс

        return id;
    }

    /// <inheritdoc/>
    public async Task<Guid> DeleteProductsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken) 
            ?? throw new Exception($"Товар с ИД:{id} не найден.");

        entity.IsDeleted = true;
        entity.UpdateDateTime = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return id;
    }
}
