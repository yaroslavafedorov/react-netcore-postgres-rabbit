using CatalogService.Services.Contracts.Dtos;
using CatalogService.Services.Contracts.Filters;

namespace CatalogService.Services.Contracts.Interfaces;

public interface ICategoryQueries
{
    /// <summary>
    /// Получение категории по идентификатору
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    Task<CategoryDto?> GetCategoryByIdAsync(Guid Id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение списка категорий по условию в фильтре
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CategoryFilter? filter, CancellationToken cancellationToken);
}