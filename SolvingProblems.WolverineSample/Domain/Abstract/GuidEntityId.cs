namespace SolvingProblems.WolverineSample.Domain.Abstract;

public abstract record GuidEntityId : EntityId
{
    protected Guid Id { get; set; }

    public GuidEntityId(Guid id)
    {
        this.Id = id;
    }

    public Guid AsGuid() => this.Id;
    public string AsString() => this.Id.ToString();

    public static implicit operator Guid(GuidEntityId id) => id.Id;
}
