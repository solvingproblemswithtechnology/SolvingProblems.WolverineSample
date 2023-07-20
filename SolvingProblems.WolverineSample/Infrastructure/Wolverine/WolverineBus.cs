using SolvingProblems.WolverineSample.Domain.Abstract;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Infrastructure.Application.Events;
using DeliveryOptions = Wolverine.DeliveryOptions;
using IMessageBus = Wolverine.IMessageBus;

namespace SolvingProblems.WolverineSample.Infrastructure.Wolverine;

public class WolverineBus : ICommandBus, IQueryBus, IIntegrationEventBus, IDomainEventBus
{
    private readonly IMessageBus messageBus;

    public WolverineBus(IMessageBus messageBus) => this.messageBus = messageBus;

    /// <summary>
    /// In memory processing of a command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellation"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public Task ProcessAsync(ICommand command, CancellationToken cancellation = default, TimeSpan? timeout = null)
        => this.messageBus.InvokeAsync(command, cancellation, timeout);

    /// <summary>
    /// Send the command to the bus for processing.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public ValueTask SendAsync(ICommand command, DeliveryOptions? options = null)
        => messageBus.SendAsync(command, options);


    /// <summary>
    /// In process processing of a query.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="query"></param>
    /// <param name="cancellation"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public Task<TResponse> ProcessAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellation = default, TimeSpan? timeout = null) => messageBus.InvokeAsync<TResponse>(query, cancellation, timeout);


    /// <summary>
    /// In process processing of an event.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="cancellation"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public Task ProcessAsync(IIntegrationEvent integrationEvent, CancellationToken cancellation = default, TimeSpan? timeout = null) => messageBus.InvokeAsync(integrationEvent, cancellation, timeout);

    /// <summary>
    /// Emit the event to the bus for processing.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public ValueTask EmitAsync(IIntegrationEvent integrationEvent, DeliveryOptions? options = null) => messageBus.PublishAsync(integrationEvent, options);

    /// <summary>
    /// Emit the event for in process processing.
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task EmitAsync(IDomainEvent domainEvent, CancellationToken cancellation = default) => messageBus.InvokeAsync(domainEvent);
}
