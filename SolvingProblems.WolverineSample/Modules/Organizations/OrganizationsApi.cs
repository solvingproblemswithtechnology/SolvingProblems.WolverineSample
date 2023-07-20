using Microsoft.AspNetCore.Mvc;
using Optional;
using SolvingProblems.WolverineSample.Infrastructure.Application;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Modules.Organizations.Application;
using SolvingProblems.WolverineSample.Modules.Organizations.Application.Dtos;

namespace SolvingProblems.WolverineSample.Modules.Organizations;

public class OrganizationsApi : Api
{
    private const string baseUri = "/organizations";

    /// <summary>
    /// Add permissions, or services needed by this module
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public override IServiceCollection ConfigureServices(IServiceCollection services) => services;

    public override void Map(IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api")
            .WithOpenApi()
            .WithName("Organizations")
            .WithTags("Organizations");

        api.MapGet(baseUri, ([FromServices] IQueryBus bus) => bus.ProcessAsync(new GetOrganizations.Query()))
            .WithName("Get Organizations")
            .Produces<IEnumerable<OrganizationDto>>()
            .ProducesProblem(404);

        api.MapGet(baseUri + "/{organizationId:guid}", async ([AsParameters] GetOrganizationById.Query query, [FromServices] IQueryBus bus) =>
        {
            Option<GetOrganizationById.Response> response = await bus.ProcessAsync(query);
            return response.Match(r => Results.Ok(r), () => Results.NotFound());
        })
        .WithName("Get Organization")
        .Produces<OrganizationDto>()
        .ProducesProblem(404);

        api.MapPost(baseUri + "/{organizationId:guid}", async ([AsParameters] RegisterOrganization.Command command, [FromServices] ICommandBus bus) =>
        {
            await bus.ProcessAsync(command);
            return TypedResults.Created($"/organizations/{command.OrganizationId}");
        })
        .WithName("Register Organization")
        .ProducesProblem(404);

        api.MapPut(baseUri + "/{organizationId:guid}", async ([AsParameters] UpdateOrganization.Command command, [FromServices] ICommandBus bus) =>
        {
            await bus.ProcessAsync(command);
            return TypedResults.NoContent();
        })
        .WithName("Update Organization")
        .ProducesProblem(404);

        api.MapGet(baseUri + "/{organizationId:guid}/users", async ([AsParameters] GetUsersFromOrganization.Query query, [FromServices] IQueryBus bus) =>
        {
            var response = await bus.ProcessAsync(query);
            return TypedResults.Ok(response);
        })
        .WithName("Get Users from Organization")
        .ProducesProblem(404);

        api.MapGet(baseUri + "/{organizationId:guid}/users/{userId:guid}", async ([AsParameters] GetUserFromOrganizationById.Query query, [FromServices] IQueryBus bus) =>
        {
            var response = await bus.ProcessAsync(query);
            return TypedResults.Ok(response);
        })
        .WithName("Get User from Organization")
        .ProducesProblem(404);

        api.MapPut(baseUri + "/{organizationId:guid}/users/{userId:guid}", async ([AsParameters] UpdateOrganization.Command command, [FromServices] ICommandBus bus) =>
        {
            await bus.ProcessAsync(command);
            return TypedResults.NoContent();
        })
        .WithName("Update User")
        .ProducesProblem(404);
    }
}