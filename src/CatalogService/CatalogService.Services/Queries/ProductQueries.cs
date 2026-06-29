using CatalogService.Domain.Entities;
using CatalogService.Persistence;
using CatalogService.Services.Contracts.Dtos;
using CatalogService.Services.Contracts.Filters;
using CatalogService.Services.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Services.Queries;

public class ProductQueries(CatalogDbContext dbContext) : IProductQueries
{
    /// <inheritdoc/>
    public async Task<ProductDto?> GetProductByIdAsync(Guid Id, CancellationToken cancellationToken)
    {
        var result = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(ToDto)
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            return null;
        }

        return result;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<ProductDto>> GetProductsAsync(ProductFilter? filter, CancellationToken cancellationToken)
    {
        var query = dbContext.Products
            .AsNoTracking()
            .AsQueryable();

        if (filter != null)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(x => x.Name == filter.Name);
            }

            if (!string.IsNullOrWhiteSpace(filter.Description))
            {
                query = query.Where(x => x.Description == filter.Description);
            }

            if (filter.CategoryId != null)
            {
                query = query.Where(x => x.CategoryId == filter.CategoryId);
            }

            if (filter.Skip != null)
            {
                query = query.Skip(filter.Skip.Value);
            }

            if (filter.Take != null)
            {
                query = query.Take(filter.Take.Value);
            }
        }

        var result = await query.Select(ToDto).ToListAsync(cancellationToken);
            

        return result;
    }

    private static readonly System.Linq.Expressions.Expression<Func<Product, ProductDto>> ToDto = 
        x => new ProductDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
            CategoryId = x.CategoryId
        };
}