using SolvingProblems.WolverineSample.Domain.Abstract;

namespace SolvingProblems.WolverineSample.Modules.Shared.Domain;

/// <summary>
/// Represents a valid Phone number in the system.
/// </summary>
public record PhoneNumber : ValueObject
{
    /// <summary>
    /// Move into a country reference
    /// </summary>
    public string Prefix { get; private set; }
    public string Number { get; private set; }

    private PhoneNumber(string prefix, string number)
    {
        this.Prefix = prefix;
        this.Number = number;
    }

    // Move into a Domain service that applies validations with a infra dependency.
    public static PhoneNumber From(string prefix, string number)
        => new PhoneNumber(prefix, number);

    public string AsString() => this.Number;
}
