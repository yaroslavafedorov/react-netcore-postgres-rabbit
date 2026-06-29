namespace CatalogService.Services.Contracts.Dtos;

/// <summary>
/// DTO Создание нового товара
/// </summary>
/// <param name="Name">Наименование товара</param>
/// <param name="Description">Описание товара</param>
/// <param name="Price">Цена товара</param>
/// <param name="CategoryId">ИД категории товара</param>
public record CreateProductRequestDto(string Name, string Description, decimal Price, Guid CategoryId);
