namespace CatalogService.Services.Contracts.Dtos;

/// <summary>
/// Продукт
/// </summary>
public record ProductDto
{
    /// <summary>
    /// ИД Продукта
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Наименование продукта
    /// </summary>
    public string Name { get; init; } = string.Empty; 

    /// <summary>
    /// Описание продукта
    /// </summary>
    public string Description { get; init; } = string.Empty; 

    /// <summary>
    /// Цена продукта
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// ИД Категории продукта
    /// </summary>
    public Guid CategoryId { get; init; }
}