using CatalogService.Services.Contracts.Dtos;
using CatalogService.Services.Contracts.Filters;

namespace CatalogService.Services.Contracts.Interfaces;

public interface IProductQueries
{
    /// <summary>
    /// Получение продкута по идентификатору
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    Task<ProductDto?> GetProductByIdAsync(Guid Id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение списка продуктов по условию в фильтре
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<ProductDto>> GetProductsAsync(ProductFilter? filter, CancellationToken cancellationToken);
}