using JasperFx.Core;
using JasperFx.Core.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MoreLinq;
using Oakton;
using SolvingProblems.WolverineSample.Domain.Abstract;
using SolvingProblems.WolverineSample.Infrastructure.Application;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Infrastructure.Application.Events;
using SolvingProblems.WolverineSample.Infrastructure.AspNetCore;
using SolvingProblems.WolverineSample.Infrastructure.Data;
using SolvingProblems.WolverineSample.Infrastructure.Data.Shared;
using SolvingProblems.WolverineSample.Infrastructure.Wolverine;
using SolvingProblems.WolverineSample.Modules.Orders;
using SolvingProblems.WolverineSample.Modules.Organizations;
using System.Reflection;
using System.Text;
using Wolverine;
using Wolverine.AzureServiceBus;
using ICommand = SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs.ICommand;

var builder = WebApplication.CreateBuilder(args);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

WolverineSettings wolverineSettings = builder.AddTypedOptions<WolverineSettings>();

builder.Services.AddHealthChecks();
builder.Services.AddCors();
builder.Services.AddLogging(configure => configure.AddConsole());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();
    options.CustomSchemaIds(type => type.NameInCode());
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();

builder.Services.AddWolverineBus();

builder.Services.AddSmartDbContext<BackendDbContext>(builder.Configuration.GetConnectionString("BackendDb")!);

builder.Services.AddApi<OrdersApi>();
builder.Services.AddApi<OrganizationsApi>();

var types = Assembly.GetAssembly(typeof(Program))!.ExportedTypes;
var domainEventTypes = types.Where(t => !t.IsInterface && t.IsAssignableTo(typeof(IDomainEvent)));
var domainEventHandlerTypes = types
    .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler<>)))
    .ToDictionary(t => t.GetGenericArguments()[0]);
var unhandledDomainEventTypes = domainEventTypes
    .Where(t => !domainEventHandlerTypes.ContainsKey(t))
    .ToList();

builder.Host.UseWolverine(options =>
{
    foreach (var unhandled in unhandledDomainEventTypes)
    {
        options.Discovery.IncludeType(typeof(NoHandler<>).MakeGenericType(unhandled));
    }

    options.OptimizeArtifactWorkflow();

    options.UseAzureServiceBus(builder.Configuration.GetConnectionString("AzureServiceBus")!)
        .PrefixIdentifiers(wolverineSettings.Prefix)
        .UseNewConventionRouting(options, conventions =>
        {
            conventions.IncludeTypes(t => t.IsAssignableTo(typeof(ICommand)) || t.IsAssignableTo(typeof(IQuery<>)) || t.IsAssignableTo(typeof(IIntegrationEvent)));

            conventions.IdentifierForSender(t => t.NameInCode());
            conventions.IdentifierForListener(t => t.NameInCode());

            conventions.UsePublishingBroadcastFor(t => t.IsAssignableTo(typeof(IIntegrationEvent)), t => "OrdersApiSub");
        })
        .AutoProvision();
});

builder.Host.ApplyOaktonExtensions();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors(configure => configure.AllowAnyOrigin().AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/api"), builder => builder.UseHttpLogging());

app.UseApis();

app.MapHealthChecks("/health-startup", GetHealthCheckOptionsForTag("startup"));
app.MapHealthChecks("/health-readiness", GetHealthCheckOptionsForTag("readiness"));
app.MapHealthChecks("/health-liveness", GetHealthCheckOptionsForTag("liveness"));

return await app.RunOaktonCommands(args);

static HealthCheckOptions GetHealthCheckOptionsForTag(string targetTag)
{
    return new HealthCheckOptions
    {
        Predicate = h => h.Tags.Count == 0 || h.Tags.Contains(targetTag),
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        },
        ResponseWriter = (context, report) =>
        {
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync(report.Status.ToString());
        }
    };
}