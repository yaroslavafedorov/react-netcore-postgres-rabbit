namespace CatalogService.Services.Contracts.Dtos;

/// <summary>
/// Изменение параметров товара
/// </summary>
/// <param name="Name">Наименование товара</param>
/// <param name="Description">Описание товара</param>
/// <param name="Price">Цена товара</param>
public record UpdateProductRequestDto(string Name, string Description, decimal Price);
