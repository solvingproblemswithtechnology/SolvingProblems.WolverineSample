namespace SolvingProblems.WolverineSample.Domain.Abstract;

public interface IDomainEventEntity
{
    /// <summary>
    /// Entity's DomainEvent
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Clears all the events in the entity.
    /// </summary>
    /// <param name="eventItem"></param>
    void RemoveDomainEvent(IDomainEvent eventItem);

    /// <summary>
    /// Clears all the events in the entity.
    /// </summary>
    /// <param name="eventItem"></param>
    void ClearDomainEvents();
}
