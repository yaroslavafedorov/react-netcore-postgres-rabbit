using CatalogService.Services.Contracts.Dtos;
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
        group.MapPost("", async (CreateProductRequestDto request, IProductCommands commands, CancellationToken cancellationToken) =>
        {
            var result = await commands.AddProductAsync(request, cancellationToken);
            return Results.Created($"/api/products/{result}", new { Id = result, Status = "Created" });
        })
        .WithName("CreateProduct");

        // PUT - Обновить продукт
        group.MapPut("/{id:guid}", async (Guid id, UpdateProductRequestDto request, IProductCommands commands, CancellationToken cancellationToken) =>
        {
            var result = await commands.UpdateProductsync(id, request, cancellationToken);
            return Results.Ok(new { Id = result, Status = "Updated" });
        })
        .WithName("UpdateProduct");

        // DELETE - Удалить продукт
        group.MapDelete("/{id:guid}", async (Guid id, IProductCommands commands, CancellationToken cancellationToken) =>
        {
            var result = await commands.DeleteProductsync(id, cancellationToken);
            return Results.Ok(new { Id = result, Status = "Deleted (Soft)" });
        })
        .WithName("DeleteProduct");

        return app;
    }
}
