namespace CatalogService.Services.Contracts.Dtos;

/// <summary>
/// Категория продуктов
/// </summary>
/// <param name="Id">ИД Категории</param>
/// <param name="Name">Наименование категории</param>
/// <param name="Description">Описание категории</param>
public record CategoryDto(Guid Id, string Description, string Name);