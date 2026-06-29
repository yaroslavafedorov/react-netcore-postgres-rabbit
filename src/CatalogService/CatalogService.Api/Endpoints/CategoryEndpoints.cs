using CatalogService.Services.Contracts.Filters;
using CatalogService.Services.Contracts.Interfaces;

namespace CatalogService.Api.Endpoints;

public static class CategoryEndpoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories").WithTags("Categories");

        // GET - Получить одну категорию
        group.MapGet("/{id:guid}", async (Guid id, ICategoryQueries queries, CancellationToken cancellationToken) =>
        {
            var result = await queries.GetCategoryByIdAsync(id, cancellationToken);

            if (result == null)
            {
                return Results.NotFound($"Не найдена категория с ИД: {id}");

            }
            return Results.Ok(result);
        })
        .WithName("GetCategory");

        // GET RANGE - Получить список категорий
        group.MapGet("", async ([AsParameters] CategoryFilter filter, ICategoryQueries queries, CancellationToken cancellationToken) =>
        {
            var result = await queries.GetCategoriesAsync(filter, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("GetCategoriesRange");

        // POST - Создать новую категорию (используем record для красивой схемы в Swagger)
        group.MapPost("", (CreateCategoryRequest request) =>
        {
            var newId = Guid.NewGuid();
            return Results.Created($"/api/categories/{newId}", new { Id = newId, Status = "Created" });
        })
        .WithName("CreateCategory");

        // PUT - Обновить категорию
        group.MapPut("/{id:guid}", (Guid id, UpdateCategoryRequest request) =>
        {
            return Results.Ok(new { Id = id, Status = "Updated" });
        })
        .WithName("UpdateCategory");

        // DELETE - Удалить категорию
        group.MapDelete("/{id:guid}", (Guid id) =>
        {
            return Results.Ok(new { Id = id, Status = "Deleted (Soft)" });
        })
        .WithName("DeleteCategory");

        return app;
    }
}

// Временные контракты (DTO) для отображения полей в интерфейсе Swagger
public record CreateCategoryRequest(string Description);
public record UpdateCategoryRequest(string Description);
