using Wolverine;

namespace SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;

public interface ICommandHandler<TCommand> : IWolverineHandler
{
    public Task Handle(TCommand command);
}
