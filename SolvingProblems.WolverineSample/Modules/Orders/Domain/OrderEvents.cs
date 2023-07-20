using SolvingProblems.WolverineSample.Domain.Abstract;

namespace SolvingProblems.WolverineSample.Modules.Orders.Domain;

public record OrderCreated(OrderId OrderId) : IDomainEvent;
public record OrderPurchased(OrderId OrderId, string ExternalThirdPartyReference) : IDomainEvent;
public record OrderPaid(OrderId OrderId) : IDomainEvent;