namespace SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;

public interface IQueryBus
{
    /// <summary>
    /// In memory processing of a query.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="query"></param>
    /// <param name="cancellation"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    Task<TResponse> ProcessAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellation = default, TimeSpan? timeout = null);
}
