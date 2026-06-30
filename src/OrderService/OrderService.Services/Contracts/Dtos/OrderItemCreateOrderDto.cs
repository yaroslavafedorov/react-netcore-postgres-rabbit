namespace OrderService.Services.Contracts.Dtos;

public record OrderItemCreateOrderDto
{
    /// <summary>
    /// ИД Товара
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// Количество
    /// </summary>
    public int Quantity { get; init; }
}