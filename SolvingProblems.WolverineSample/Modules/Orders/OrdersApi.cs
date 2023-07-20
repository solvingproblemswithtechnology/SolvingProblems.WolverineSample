using Microsoft.AspNetCore.Mvc;
using Optional;
using SolvingProblems.WolverineSample.Infrastructure.Application;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Modules.Orders.Application;

namespace SolvingProblems.WolverineSample.Modules.Orders;

public class OrdersApi : Api
{
    private const string baseUri = "/orders";

    public override IServiceCollection ConfigureServices(IServiceCollection services) => services;

    public override void Map(IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api")
            .WithOpenApi()
            .WithName("Orders")
            .WithTags("Orders");

        api.MapGet(baseUri, async ([AsParameters] GetOrdersFiltered.Query query, [FromServices] IQueryBus bus) =>
        {
            GetOrdersFiltered.Response response = await bus.ProcessAsync(query);
            return TypedResults.Ok(response);
        })
        .WithName("Get Orders")
        .ProducesProblem(404);

        api.MapGet(baseUri + "/{orderId:guid}", async ([AsParameters] GetOrder.Query query, [FromServices] IQueryBus bus) =>
        {
            Option<GetOrder.Response> response = await bus.ProcessAsync(query);
            return response.Match(r => TypedResults.Ok(r), () => Results.NotFound());
        })
        .WithName("Get Order")
        .Produces<GetOrder.Response>()
        .ProducesProblem(404);

        api.MapPost(baseUri + "/{orderId:guid}", async ([AsParameters] PurchaseOrder.Command command, [FromServices] ICommandBus bus) =>
        {
            await bus.ProcessAsync(command);
            return TypedResults.Created($"/orders/{command.OrderId}");
        })
        .WithName("Purchase Order")
        .ProducesProblem(404);
    }
}