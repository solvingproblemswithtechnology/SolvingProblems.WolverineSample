using SolvingProblems.WolverineSample.Domain.Abstract;
using Wolverine;

namespace SolvingProblems.WolverineSample.Infrastructure.Application.Events;

public interface IDomainEventHandler<TDomainEvent> : IWolverineHandler where TDomainEvent : IDomainEvent
{
    public ValueTask Handle(TDomainEvent e);
}