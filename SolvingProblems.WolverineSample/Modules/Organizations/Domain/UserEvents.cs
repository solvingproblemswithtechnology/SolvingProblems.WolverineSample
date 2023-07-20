using SolvingProblems.WolverineSample.Domain.Abstract;

namespace SolvingProblems.WolverineSample.Modules.Organizations.Domain;

public record UserCreated(UserId UserId) : IDomainEvent;
public record UserMainInformationModified(UserId UserId) : IDomainEvent;
public record UserExtraInformationModified(UserId UserId) : IDomainEvent;
public record UserSimAssigned(UserId UserId) : IDomainEvent;
public record UserRoleAssigned(UserId UserId) : IDomainEvent;