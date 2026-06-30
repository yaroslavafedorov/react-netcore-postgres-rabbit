namespace CatalogService.Services.Contracts.Configurations;

/// <summary>
/// Настройки подключения к брокеру сообщений RabbitMQ
/// </summary>
public record MessageBusSettings
{
    public string Host { get; init; } = "localhost";
    public ushort Port { get; init; } = 5672;
    public string Username { get; init; } = "guest";
    public string Password { get; init; } = "guest";
}
