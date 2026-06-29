namespace CatalogService.Services.Contracts.Filters;

/// <summary>
/// Фильтр для поиска продукта
/// </summary>
public class ProductFilter : PagingFilter
{
    /// <summary>
    /// Наименование продукта
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Описание продукта
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// ИД Категории продуктов
    /// </summary>
    public Guid? CategoryId { get; set; }
}