using CatalogService.Api.Endpoints;
using CatalogService.Persistence;
using CatalogService.Persistence.Seeds;
using CatalogService.Services.Commands;
using CatalogService.Services.Contracts.Interfaces;
using CatalogService.Services.Queries;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using CatalogService.Services.Contracts.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Отключаем строгую проверку графа DI при запуске из-под инструментов миграции CLI
builder.Host.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = false;
    options.ValidateOnBuild = false;
});

// 1. Извлекаем строку подключения из appsettings.json
var connectionString = builder.Configuration.GetConnectionString("Connection");

// 2. Регистрируем контекст БД в DI-контейнере с провайдером PostgreSQL
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("CatalogService.Persistence")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Catalog Service API", Version = "v1" });
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
// Категории
builder.Services.AddScoped<ICategoryQueries, CategoryQueries>();
// Товары
builder.Services.AddScoped<IProductCommands, ProductCommands>();
builder.Services.AddScoped<IProductQueries, ProductQueries>();

// =========================================================================
// НАСТРОЙКА MASSTRANSIT + RABBITMQ + OUTBOX
// =========================================================================
var busSettings = builder.Configuration
    .GetSection("MessageBusSettings")
    .Get<MessageBusSettings>() ?? new MessageBusSettings();
    
builder.Services.AddMassTransit(x =>
{
    // 1. Привязываем MassTransit к  контексту базы данных для работы Outbox
    x.AddEntityFrameworkOutbox<CatalogDbContext>(o =>
    {
        o.UsePostgres();   // Оптимизация под синтаксис PostgreSQL
        o.UseBusOutbox();  // Автоматический запуск воркера доставки сообщений
    });

    // Форматируем имена очередей в kebab-case (например, product-price-changed-event)
    x.SetKebabCaseEndpointNameFormatter();

    // 2. Настраиваем транспорт для отправки (RabbitMQ)
    x.UsingRabbitMq((context, cfg) =>
    {
        // Указываем адрес контейнера из Docker Compose
        cfg.Host(busSettings.Host, busSettings.Port, "/", h =>
        {
            h.Username(busSettings.Username);
            h.Password(busSettings.Password);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API v1"));
}

// Включаем CORS
app.UseCors("ReactAppPolicy");

await CatalogDbInitializer.SeedAsync(app.Services);

app.MapCategoryEndpoints();
app.MapProductEndpoints(); 

app.Run();
