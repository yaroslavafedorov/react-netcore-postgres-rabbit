using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.Endpoints;
using OrderService.Persistence;
using OrderService.Services.Commands;
using OrderService.Services.Consumers;
using OrderService.Services.Contracts.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("OrderService.Persistence")));
    
// Извлекаем настройки RabbitMQ из appsettings.json
var rabbitHost = builder.Configuration.GetValue<string>("MessageBusSettings:Host") ?? "localhost";
var rabbitPort = builder.Configuration.GetValue<ushort>("MessageBusSettings:Port") == 0 ? (ushort)5672 : builder.Configuration.GetValue<ushort>("MessageBusSettings:Port");
var rabbitUser = builder.Configuration.GetValue<string>("MessageBusSettings:Username") ?? "guest";
var rabbitPass = builder.Configuration.GetValue<string>("MessageBusSettings:Password") ?? "guest";

// НАСТРОЙКА MASSTRANSIT ДЛЯ ПРИЕМА СООБЩЕНИЙ
builder.Services.AddMassTransit(x =>
{
    // 1. Регистрируем наш класс-консьюмер в контейнере
    x.AddConsumer<ProductPriceChangedConsumer>();

    // Подключаем Transactional Inbox/Outbox на основе нашего OrderDbContext
    x.AddEntityFrameworkOutbox<OrderDbContext>(o =>
    {
        o.UsePostgres();   // Оптимизация блокировок под PostgreSQL
        o.UseBusOutbox();  // Автоматический запуск воркера
    });

    x.SetKebabCaseEndpointNameFormatter();

    // 2. Настраиваем подключение к RabbitMQ
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitHost, rabbitPort, "/", h =>
        {
            h.Username(rabbitUser);
            h.Password(rabbitPass);
        });

        // Автоматически настраивает эндпоинты (очереди) для всех зарегистрированных консьюмеров
        // Для ProductPriceChangedConsumer создастся очередь "product-price-changed"
        cfg.ConfigureEndpoints(context);
    });
});

// CQRS
// Заказы
builder.Services.AddScoped<IOrderCommands, OrderCommands>();

var app = builder.Build();

app.MapOrderEndpoints(); 

app.Run();
