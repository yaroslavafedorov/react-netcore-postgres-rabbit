using OrderService.Domain.Common;

namespace OrderService.Domain.Entities;

/// <summary>
/// Продукт
/// </summary>
public class Product : BaseItem
{
    /// <summary>
    /// ИД Продукта
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Цена продукта
    /// </summary>
    public decimal Price { get; set; }
}