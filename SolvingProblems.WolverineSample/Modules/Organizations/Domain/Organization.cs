using SolvingProblems.WolverineSample.Domain.Abstract;
using SolvingProblems.WolverineSample.Modules.Shared.Domain;

namespace SolvingProblems.WolverineSample.Modules.Organizations.Domain;

public record OrganizationId : GuidEntityId
{
    public OrganizationId(Guid id) : base(id) { }
}

public class Organization : AggregateRoot<OrganizationId>
{
    public enum Status { Created = 0, Ready = 10 }

    public string Name { get; private set; } = null!;

    // Candidates for another Entity
    public string? TaxId { get; private set; }
    public string? TaxBusinessName { get; private set; }
    public string? TaxFirstName { get; private set; }
    public string? TaxLastName { get; private set; }
    public string? TaxAddress { get; private set; }
    public virtual Email? TaxEmail { get; private set; }

    public Status OrganizationStatus { get; private set; }

    /// <summary>
    /// Spending limit for the whole organization. To be promoted to Orders module.
    /// </summary>
    public decimal MonthlyBudgetLimit { get; set; }

    private readonly List<User> users = new List<User>();
    public virtual IReadOnlyCollection<User> Users => this.users.AsReadOnly();

    /// <summary>
    /// For EntityFramework
    /// </summary>
    /// <param name="Id"></param>
    private Organization(OrganizationId Id) : base(Id) { }

    /// <summary>
    /// Creates a new organization with the minimum fields allowed
    /// </summary>
    /// <param name="organizationId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Organization CreateOrganization(OrganizationId organizationId, string name)
    {
        Organization organization = new Organization(organizationId) { Name = name, OrganizationStatus = Status.Created, MonthlyBudgetLimit = 0 };
        organization.AddDomainEvent(new OrganizationCreated(organization.Id));

        return organization;
    }

    public void ReviewOrganization(decimal initialTotalSpendingLimit)
    {
        this.MonthlyBudgetLimit = initialTotalSpendingLimit;
        this.OrganizationStatus = Status.Ready;

        this.AddDomainEvent(new OrganizationReviewed(this.Id));
    }

    /// <summary>
    /// Modifies the tax information
    /// </summary>
    /// <param name="taxId"></param>
    /// <param name="taxAddress"></param>
    public void ModifyTaxInformation(string taxId, string taxBusinessName, string taxFirstName, string taxLastName, string taxAddress, Email taxEmail)
    {
        this.TaxId = taxId;
        this.TaxBusinessName = taxBusinessName;
        this.TaxFirstName = taxFirstName;
        this.TaxLastName = taxLastName;
        this.TaxAddress = taxAddress;
        this.TaxEmail = taxEmail;

        this.AddDomainEvent(new OrganizationTaxInformationModified(this.Id));
    }

    /// <summary>
    /// Adds an user to the organization
    /// </summary>
    /// <param name="user"></param>
    public void AddUser(User user)
    {
        this.users.Add(user);

        this.AddDomainEvent(new OrganizationUserAdded(this.Id, user.Id));
    }

    internal void ReviewOrganization(object newLimit) => throw new NotImplementedException();
}
