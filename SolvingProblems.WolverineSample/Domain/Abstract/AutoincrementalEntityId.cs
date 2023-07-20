namespace SolvingProblems.WolverineSample.Domain.Abstract;

public record AutoIncrementalEntityId : EntityId
{
    public long Id { get; protected set; }

    private AutoIncrementalEntityId() { }

    public AutoIncrementalEntityId(long id)
    {
        this.Id = id;
    }

    public long AsLong() => this.Id;
    public string AsString() => this.Id.ToString();

    public static explicit operator long(AutoIncrementalEntityId id) => id.AsLong();
    public static implicit operator AutoIncrementalEntityId(long id) => new AutoIncrementalEntityId(id);
}
