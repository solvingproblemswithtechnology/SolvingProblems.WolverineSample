using SolvingProblems.WolverineSample.Domain.Abstract;
using SolvingProblems.WolverineSample.Infrastructure.Application.Events;

namespace SolvingProblems.WolverineSample.Infrastructure.Application;

public class NoHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    public ValueTask Handle(TEvent request) => ValueTask.CompletedTask;
}
