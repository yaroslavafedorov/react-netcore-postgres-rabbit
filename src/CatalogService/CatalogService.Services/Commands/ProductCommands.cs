using CatalogService.Domain.Entities;
using CatalogService.Persistence;
using CatalogService.Services.Contracts.Dtos;
using CatalogService.Services.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using CatalogService.Services.Contracts.Events;

namespace CatalogService.Services.Commands;

public class ProductCommands(
    CatalogDbContext dbContext,
    IPublishEndpoint publishEndpoint) : IProductCommands
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
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        {
            try
            {
                var entity = await dbContext.Products
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                    ?? throw new Exception($"Товар с ИД:{id} не найден.");

                var isPriceChanged = entity.Price != product.Price;

                entity.Name = product.Name;
                entity.Description = product.Description;
                entity.Price = product.Price;
                entity.UpdateDateTime = DateTimeOffset.UtcNow;

                await dbContext.SaveChangesAsync(cancellationToken);

                if (isPriceChanged)
                {
                    await publishEndpoint.Publish(new ProductPriceChangedEvent
                    {
                        ProductId = entity.Id,
                        Price = entity.Price,
                        Name = entity.Name
                    }, cancellationToken);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

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
