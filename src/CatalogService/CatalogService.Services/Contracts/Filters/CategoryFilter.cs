namespace CatalogService.Services.Contracts.Filters;

/// <summary>
/// Фильтр для поиска категории
/// </summary>
public class CategoryFilter : PagingFilter
{
    /// <summary>
    /// Наименование категории
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Описание категории
    /// </summary>
    public string? Description { get; set; }
}