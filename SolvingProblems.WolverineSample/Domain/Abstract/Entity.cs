using System.ComponentModel.DataAnnotations;

namespace SolvingProblems.WolverineSample.Domain.Abstract;

/// <summary>
/// Represents an Entity of the Domain
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class Entity<TIdentifier> : IEquatable<Entity<TIdentifier>>, IAuditable
    where TIdentifier : EntityId
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    [Key]
    public virtual TIdentifier Id { get; protected set; }

    public virtual DateTimeOffset CreationDateTime { get; private set; }
    public virtual DateTimeOffset UpdateDateTime { get; private set; }

    /// <summary>
    /// Id can't be null never.
    /// Entity framework will use this constructor for building it.
    /// </summary>
    /// <param name="Id"></param>
    protected Entity(TIdentifier Id)
    {
        this.Id = Id;
    }

    #region Equals

    /// <summary>
    /// Equals using the Id if it's an entity
    /// </summary>
    /// <param name="obj">Object to compare</param>
    public override bool Equals(object? obj) => Equals(obj as Entity<TIdentifier>);

    /// <summary>
    /// Equals using the Id
    /// </summary>
    /// <param name="other">Entity to compare</param>
    public bool Equals(Entity<TIdentifier>? other) => other is not null && this.Id == other.Id;

    /// <summary>
    /// Override. Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => 2108858624 + this.Id.GetHashCode();

    /// <summary>
    /// Overriden for true equality (No reference equality)
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Entity<TIdentifier> left, Entity<TIdentifier> right)
        => EqualityComparer<Entity<TIdentifier>>.Default.Equals(left, right);

    /// <summary>
    /// Overriden for true equality (No reference equality)
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public static bool operator !=(Entity<TIdentifier> left, Entity<TIdentifier> right) => !(left == right);

    #endregion
}
