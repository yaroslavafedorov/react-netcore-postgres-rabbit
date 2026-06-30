using OrderService.Services.Contracts.Dtos;
using OrderService.Services.Contracts.Interfaces;

namespace OrderService.Api.Endpoints;

public static class OrderEndpointsdpoints
{
    
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders");

        // POST - Создать новый заказ (используем record для красивой схемы в Swagger)
        group.MapPost("", async (CreateOrderRequestDto request, IOrderCommands commands, CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await commands.CreateOrderAsync(request, cancellationToken);
                return Results.Created($"/api/orders/{result}", new { Id = result, Status = "Created" });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(ex);
            }
        })
        .WithName("CreateOrder");

        return app;
    }
}
