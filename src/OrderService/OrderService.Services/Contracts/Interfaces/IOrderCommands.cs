using OrderService.Services.Contracts.Dtos;

namespace OrderService.Services.Contracts.Interfaces;

/// <summary>
/// Команды для работы с Заказами
/// </summary>
public interface IOrderCommands
{
    /// <summary>
    /// Создание нового заказа
    /// </summary>
    /// <param name="order">Данные заказа</param>
    /// <returns></returns>
    Task<Guid> CreateOrderAsync(CreateOrderRequestDto order, CancellationToken cancellationToken);
}
