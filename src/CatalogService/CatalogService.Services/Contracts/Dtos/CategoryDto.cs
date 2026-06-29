namespace CatalogService.Services.Contracts.Dtos;

/// <summary>
/// Категория продуктов
/// </summary>
public class CategoryDto
{
    /// <summary>
    /// ИД Категории
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Описание категории
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Наименование категории
    /// </summary>
    public string Name { get; set; } = string.Empty;

}