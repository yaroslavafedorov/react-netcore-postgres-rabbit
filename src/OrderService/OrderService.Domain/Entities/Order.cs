using OrderService.Domain.Common;

namespace OrderService.Domain.Entities;

/// <summary>
/// Заказ
/// </summary>
public class Order : BaseItem
{
    /// <summary>
    /// ИД Заказа
    /// </summary>
    public Guid Id { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = [];

    /// <summary>
    /// Общая стоимость заказа
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Статус заказа
    /// </summary>
    public string Status { get; set; } = string.Empty;
}