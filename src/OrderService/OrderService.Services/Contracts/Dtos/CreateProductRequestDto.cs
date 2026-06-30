namespace OrderService.Services.Contracts.Dtos;

/// <summary>
/// DTO Создание нового заказа
/// </summary>
public record CreateOrderRequestDto
{
    /// <summary>
    /// Список позиций заказа
    /// </summary>
    public ICollection<OrderItemCreateOrderDto> Items { get; init; } = [];
}
