using System.ComponentModel.DataAnnotations.Schema;

namespace SolvingProblems.WolverineSample.Domain.Abstract;

public abstract class AggregateRoot<TIdentifier> : Entity<TIdentifier>, IDomainEventEntity, IAuditable
   where TIdentifier : EntityId
{
    /// <summary>
    /// Real list of domain events
    /// </summary>
    [NotMapped]
    private readonly Lazy<List<IDomainEvent>> domainEvents = new Lazy<List<IDomainEvent>>(() => new List<IDomainEvent>());

    protected AggregateRoot(TIdentifier Id) : base(Id) { }

    /// <summary>
    /// Entity's DomainEvent
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => !this.domainEvents.IsValueCreated
        ? Array.Empty<IDomainEvent>() : this.domainEvents.Value.AsReadOnly();

    /// <summary>
    /// Adds an event to this entity domain events.
    /// </summary>
    /// <param name="eventItem"></param>
    protected void AddDomainEvent(IDomainEvent domainEvent) => this.domainEvents.Value.Add(domainEvent);

    /// <summary>
    /// Clears all the events in the entity.
    /// </summary>
    /// <param name="eventItem"></param>
    public void RemoveDomainEvent(IDomainEvent domainEvent) => this.domainEvents.Value.Remove(domainEvent);

    /// <summary>
    /// Clears all the events in the entity. Change to a ConsumeEvents using yield and removing the event at each iteration. 
    /// </summary>
    /// <param name="eventItem"></param>
    public void ClearDomainEvents() => this.domainEvents.Value.Clear();
}
