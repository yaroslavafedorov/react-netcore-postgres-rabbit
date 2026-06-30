using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.Endpoints;
using OrderService.Persistence;
using OrderService.Services.Commands;
using OrderService.Services.Consumers;
using OrderService.Services.Contracts.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Отключаем строгую проверку графа DI при запуске из-под инструментов миграции CLI
builder.Host.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = false;
    options.ValidateOnBuild = false;
});

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Order Service API", Version = "v1" });
});

// 4. Настраиваем CORS-политику для React
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// CQRS
// Заказы
builder.Services.AddScoped<IOrderCommands, OrderCommands>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1"));
}

// Включаем CORS
app.UseCors("ReactAppPolicy");

app.MapOrderEndpoints(); 

app.Run();
