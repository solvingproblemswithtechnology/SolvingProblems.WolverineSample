using Wolverine;
using Wolverine.AzureServiceBus;

namespace SolvingProblems.WolverineSample.Infrastructure.Wolverine;

public static class WolverineExtensions
{
    public static AzureServiceBusConfiguration UseNewConventionRouting(this AzureServiceBusConfiguration configuration, WolverineOptions trickedOptions,
        Action<AzureServiceBusQueueAndTopicMessageRoutingConvention>? configure = null)
    {
        var routing = new AzureServiceBusQueueAndTopicMessageRoutingConvention();
        configure?.Invoke(routing);

        trickedOptions.RouteWith(routing);

        return configuration;
    }
}
