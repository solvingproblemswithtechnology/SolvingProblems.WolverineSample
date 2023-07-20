using SolvingProblems.WolverineSample.Domain.Abstract;

namespace SolvingProblems.WolverineSample.Modules.Organizations.Domain;

public record OrganizationCreated(OrganizationId OrganizationId) : IDomainEvent;
public record OrganizationReviewed(OrganizationId OrganizationId) : IDomainEvent;
public record OrganizationTaxInformationModified(OrganizationId OrganizationId) : IDomainEvent;
public record OrganizationUserAdded(OrganizationId OrganizationId, UserId UserId) : IDomainEvent;
