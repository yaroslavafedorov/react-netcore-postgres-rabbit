using OrderService.Domain.Common;

namespace OrderService.Domain.Entities;

/// <summary>
/// Позиция заказа
/// </summary>
public class OrderItem : BaseItem
{
    /// <summary>
    /// ИД Позиции Заказа
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ИД Заказа
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// ИД Продукта
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Цена продукта
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Количество товаров
    /// </summary>
    public int Quantity { get; set; }

    public Order? Order { get; set; }

}