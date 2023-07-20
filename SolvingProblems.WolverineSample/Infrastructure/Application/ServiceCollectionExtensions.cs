using Microsoft.Extensions.DependencyInjection.Extensions;
using SolvingProblems.WolverineSample.Domain.Abstract;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Infrastructure.Application.Events;
using SolvingProblems.WolverineSample.Infrastructure.Wolverine;

namespace SolvingProblems.WolverineSample.Infrastructure.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWolverineBus(this IServiceCollection services)
    {
        services.TryAddSingleton<WolverineBus>();

        services.TryAddSingleton<ICommandBus>(s => s.GetRequiredService<WolverineBus>());
        services.TryAddSingleton<IQueryBus>(s => s.GetRequiredService<WolverineBus>());
        services.TryAddSingleton<IIntegrationEventBus>(s => s.GetRequiredService<WolverineBus>());
        services.TryAddSingleton<IDomainEventBus>(s => s.GetRequiredService<WolverineBus>());

        return services;
    }

    public static IServiceCollection AddApi<TApi>(this IServiceCollection services)
        where TApi : Api
    {
        TApi api = Activator.CreateInstance<TApi>();

        return api.ConfigureServices(services)
            .AddSingleton<Api>(api);
    }
}
