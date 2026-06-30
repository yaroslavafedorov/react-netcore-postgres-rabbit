// Пространство имен ДОЛЖНО быть строго идентичным каталогу!
// Иначе MassTransit в RabbitMQ решит, что это два разных типа сообщений.
namespace CatalogService.Services.Contracts.Events;

/// <summary>
/// Событие на изменение цены товара, прилетающее из Каталога
/// </summary>
public record ProductPriceChangedEvent
{
    public Guid ProductId { get; init; }
    public decimal Price { get; init; }
    public string Name { get; init; } = string.Empty;
}
