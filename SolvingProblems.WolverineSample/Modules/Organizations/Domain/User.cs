using SolvingProblems.WolverineSample.Domain.Abstract;
using SolvingProblems.WolverineSample.Modules.Shared.Domain;

namespace SolvingProblems.WolverineSample.Modules.Organizations.Domain;

public record UserId : GuidEntityId
{
    public UserId(Guid id) : base(id) { }
}

public class User : AggregateRoot<UserId>
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public virtual Email Email { get; private set; } = null!;
    public virtual PhoneNumber? PersonalPhoneNumber { get; private set; }
    public string? Position { get; private set; }
    public string? Department { get; private set; }

    public virtual OrganizationId OrganizationId { get; private set; } = null!;
    public virtual Organization Organization { get; private set; } = null!;

    /// <summary>
    /// Model Role
    /// </summary>
    public string Role { get; set; } = null!;

    /// <summary>
    /// For EntityFramework
    /// </summary>
    /// <param name="Id"></param>
    private User(UserId Id) : base(Id) { }

    /// <summary>
    /// Creates a new user with the minimum fields allowed
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    public static User CreateUser(UserId userId, string firstName, string lastName, Email email)
    {
        // TODO Model roles
        User user = new User(userId) { FirstName = firstName, LastName = lastName, Email = email };
        user.AddDomainEvent(new UserCreated(user.Id));

        return user;
    }

    /// <summary>
    /// Assings a valid role.
    /// </summary>
    /// <param name="role"></param>
    public void ModifyMainInformation(string firstName, string lastName, Email email)
    {
        // Move into a model
        this.FirstName = firstName ?? this.FirstName;
        this.LastName = lastName ?? this.LastName;
        this.Email = email ?? this.Email;

        this.AddDomainEvent(new UserMainInformationModified(this.Id));
    }

    /// <summary>
    /// Assings a valid role.
    /// </summary>
    /// <param name="role"></param>
    public void ModifyExtraInformation(PhoneNumber? personalPhoneNumber = default, string? position = default, string? department = default)
    {
        // Move into a model
        this.PersonalPhoneNumber = personalPhoneNumber ?? this.PersonalPhoneNumber;
        this.Position = position ?? this.Position;
        this.Department = department ?? this.Department;

        this.AddDomainEvent(new UserExtraInformationModified(this.Id));
    }

    /// <summary>
    /// Assings a valid role.
    /// </summary>
    /// <param name="role"></param>
    public void AssignRole(string role)
    {
        // Move into a model
        this.Role = role ?? "Admin";

        this.AddDomainEvent(new UserRoleAssigned(this.Id));
    }
}
