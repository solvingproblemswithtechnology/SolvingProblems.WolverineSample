using Wolverine;

namespace SolvingProblems.WolverineSample.Infrastructure.Application.Events;

public interface IIntegrationEventHandler<TIntegrationEvent> : IWolverineHandler where TIntegrationEvent : IIntegrationEvent
{
    public ValueTask Handle(TIntegrationEvent e);
}