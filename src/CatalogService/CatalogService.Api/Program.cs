using CatalogService.Api.Endpoints;
using CatalogService.Persistence;
using CatalogService.Persistence.Seeds;
using CatalogService.Services.Commands;
using CatalogService.Services.Contracts.Interfaces;
using CatalogService.Services.Queries;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
