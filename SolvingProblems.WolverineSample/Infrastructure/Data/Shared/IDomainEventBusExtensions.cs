using SolvingProblems.WolverineSample.Domain.Abstract;

namespace SolvingProblems.WolverineSample.Infrastructure.Data.Shared;

public static class IDomainEventBusExtensions
{
    /// <summary>
    /// Dispatches all Domain Events.
    /// </summary>
    /// <param name="eventBus"></param>
    /// <param name="entities"></param>
    /// <returns></returns>
    public static async Task DispatchDomainEventsAsync(this IDomainEventBus eventBus, IEnumerable<IDomainEventEntity> entities)
    {
        foreach (IDomainEventEntity entity in entities)
        {
            foreach (IDomainEvent domainEvent in entity.DomainEvents)
            {
                await eventBus.EmitAsync(domainEvent).ConfigureAwait(false);
            }

            // Remove all at once to improve performance as there's no need to delete one by one
            entity.ClearDomainEvents();
        }
    }
}
