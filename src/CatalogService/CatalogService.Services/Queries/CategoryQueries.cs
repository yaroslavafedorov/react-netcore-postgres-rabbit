using CatalogService.Domain.Entities;
using CatalogService.Persistence;
using CatalogService.Services.Contracts.Dtos;
using CatalogService.Services.Contracts.Filters;
using CatalogService.Services.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Services.Queries;

public class CategoryQueries(CatalogDbContext dbContext) : ICategoryQueries
{
    /// <inheritdoc/>
    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid Id, CancellationToken cancellationToken)
    {
        var result = await dbContext.Categories
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
    public async Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CategoryFilter? filter, CancellationToken cancellationToken)
    {
        var query = dbContext.Categories
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

    private static readonly System.Linq.Expressions.Expression<Func<Category, CategoryDto>> ToDto = 
        x => new CategoryDto
        (
            x.Id,
            x.Name,
            x.Description
        );
}