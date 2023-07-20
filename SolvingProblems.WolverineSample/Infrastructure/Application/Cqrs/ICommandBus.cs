using Wolverine;

namespace SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;

public interface ICommandBus
{
    /// <summary>
    /// In memory processing of a command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellation"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    Task ProcessAsync(ICommand command, CancellationToken cancellation = default, TimeSpan? timeout = null);

    /// <summary>
    /// Send the command to the bus for processing.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    ValueTask SendAsync(ICommand command, DeliveryOptions? options = null);
}
