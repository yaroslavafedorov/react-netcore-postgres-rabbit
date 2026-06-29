using CatalogService.Domain.Common;

namespace CatalogService.Domain.Entities;

/// <summary>
/// Категория продуктов
/// </summary>
public class Category : BaseItem
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

    public ICollection<Product> Products { get; set; }= [];
}