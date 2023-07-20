namespace SolvingProblems.WolverineSample.Domain.Abstract;

public interface IDomainEventBus
{
    Task EmitAsync(IDomainEvent domainEvent, CancellationToken cancellation = default);
}