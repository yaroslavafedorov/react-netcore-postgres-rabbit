using CatalogService.Services.Contracts.Filters;
using CatalogService.Services.Contracts.Interfaces;

namespace CatalogService.Api.Endpoints;

public static class ProductEndpoints
{
    
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products").WithTags("Products");

        // GET - Получить один продукт
        group.MapGet("/{id:guid}", async (Guid id, IProductQueries queries, CancellationToken cancellationToken) =>
        {
            var result = await queries.GetProductByIdAsync(id, cancellationToken);

            if (result != null)
            {
                return Results.Ok(result);
            }
            else
            {
                return Results.NotFound($"Продукт с ИД:{id} не найден");
            }
        })
        .WithName("GetProduct");

        // GET RANGE - Получить список продуктов
        group.MapGet("", async ([AsParameters] ProductFilter filter, IProductQueries queries, CancellationToken cancellationToken) =>
        {
            var result = await queries.GetProductsAsync(filter, cancellationToken);

            return Results.Ok(result);
        })
        .WithName("GetProductsRange");

        // POST - Создать новый продукт (используем record для красивой схемы в Swagger)
        group.MapPost("", (CreateProductRequest request) =>
        {
            var newId = Guid.NewGuid();
            return Results.Created($"/api/products/{newId}", new { Id = newId, Status = "Created" });
        })
        .WithName("CreateProduct");

        // PUT - Обновить продукт
        group.MapPut("/{id:guid}", (Guid id, UpdateProductRequest request) =>
        {
            return Results.Ok(new { Id = id, Status = "Updated" });
        })
        .WithName("UpdateProduct");

        // DELETE - Удалить продукт
        group.MapDelete("/{id:guid}", (Guid id) =>
        {
            return Results.Ok(new { Id = id, Status = "Deleted (Soft)" });
        })
        .WithName("DeleteProduct");

        return app;
    }
}

// Временные контракты (DTO) для отображения полей в интерфейсе Swagger
public record CreateProductRequest(string Name, string Description, decimal Price, Guid CategoryId);
public record UpdateProductRequest(string Name, string Description, decimal Price);
