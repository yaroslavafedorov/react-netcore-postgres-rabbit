namespace CatalogService.Services.Contracts.Dtos;

/// <summary>
/// Продукт
/// </summary>
public class ProductDto
{
    /// <summary>
    /// ИД Продукта
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Наименование продукта
    /// </summary>
    public string Name { get; set; } = string.Empty; 

    /// <summary>
    /// Описание продукта
    /// </summary>
    public string Description { get; set; } = string.Empty; 

    /// <summary>
    /// Цена продукта
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// ИД Категории продукта
    /// </summary>
    public Guid CategoryId { get; set; }
}