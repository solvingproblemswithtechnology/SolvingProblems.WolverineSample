using SolvingProblems.WolverineSample.Domain.Abstract;

namespace SolvingProblems.WolverineSample.Modules.Shared.Domain;

/// <summary>
/// Represents a valid Email in the system.
/// </summary>
public record Email : ValueObject
{
    public string Address { get; private set; }

    private Email(string address)
    {
        this.Address = address;
    }

    public static Email From(string address) => new Email(address);

    public string AsString() => this.Address;
}
