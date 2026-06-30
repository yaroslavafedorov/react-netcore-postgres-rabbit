namespace CatalogService.Services.Contracts.Events;

/// <summary>
/// Событие на изменение цены товара
/// </summary>
public record ProductPriceChangedEvent
{
    /// <summary>
    /// ИД товара
    /// </summary>
    public Guid ProductId { get; init;}

    /// <summary>
    /// Цена товара
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// Наименование товара
    /// </summary>
    public string Name { get; init; } = string.Empty;
}
