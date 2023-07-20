using SolvingProblems.WolverineSample.Modules.Organizations.Domain;

namespace SolvingProblems.WolverineSample.Modules.Organizations.Application.Dtos;

public record OrganizationDto(Guid? OrganizationId, string Name, decimal MonthlyBudgetLimit, string OrganizationStatus, string? TaxId, string? TaxBusinessName, string? TaxFirstName, string? TaxLastName, string? TaxAddress, string? TaxEmail)
{
    public static OrganizationDto FromOrganization(Organization organization) => new OrganizationDto(organization.Id.AsGuid(),
        organization.Name, organization.MonthlyBudgetLimit, organization.OrganizationStatus.ToString(), organization.TaxId, organization.TaxBusinessName, organization.TaxFirstName, organization.TaxLastName,
        organization.TaxAddress, organization.TaxEmail?.AsString());
}
