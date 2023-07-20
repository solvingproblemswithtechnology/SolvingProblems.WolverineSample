using Wolverine;

namespace SolvingProblems.WolverineSample.Infrastructure.Application.Events;

public interface IIntegrationEventBus
{
    /// <summary>
    /// In memory processing of an event.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="cancellation"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    Task ProcessAsync(IIntegrationEvent integrationEvent, CancellationToken cancellation = default, TimeSpan? timeout = null);

    /// <summary>
    /// Send the event to the bus for processing.
    /// WARN: DeliveryOptions shouldn't be here, but it's a tradeoff for now until we have more knowledge about the needs.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    ValueTask EmitAsync(IIntegrationEvent integrationEvent, DeliveryOptions? options = null);
}
