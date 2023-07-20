namespace SolvingProblems.WolverineSample.Domain.Abstract;

/// <summary>
/// Represents an entity that can be Auditable by user and date on creation and modification.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// DateTime of creation
    /// </summary>
    DateTimeOffset CreationDateTime { get; }

    /// <summary>
    /// DateTime of update
    /// </summary>
    DateTimeOffset UpdateDateTime { get; }
}
