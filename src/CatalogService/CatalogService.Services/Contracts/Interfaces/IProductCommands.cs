using CatalogService.Services.Contracts.Dtos;

namespace CatalogService.Services.Contracts.Interfaces;

/// <summary>
/// Добавление, изменение, удаление товаров
/// </summary>
public interface IProductCommands
{
    /// <summary>
    /// Создание нового товара
    /// </summary>
    /// <param name="newProduct">ДТО для создания нового товара</param>
    /// <returns>ИД записи созданного товара</returns>
    Task<Guid> AddProductAsync(CreateProductRequestDto newProduct, CancellationToken cancellationToken);

    /// <summary>
    /// Изменение параметров товара
    /// </summary>
    /// <param name="id">ИД товара</param>
    /// <param name="product">ДТО для изменения параметров товара</param>
    /// <returns>ИД записи измененного товара</returns>
    Task<Guid> UpdateProductsync(Guid id, UpdateProductRequestDto product, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление товара
    /// </summary>
    /// <param name="id">ИД товара</param>
    /// <returns>ИД записи удаленного товара</returns>
    Task<Guid> DeleteProductsync(Guid id, CancellationToken cancellationToken);
}
