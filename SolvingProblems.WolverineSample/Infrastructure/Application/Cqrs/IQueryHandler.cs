using Wolverine;

namespace SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;

public interface IQueryHandler<TRequest, TResponse> : IWolverineHandler where TRequest : IQuery<TResponse>
{
    public Task<TResponse> Handle(TRequest request);
}
