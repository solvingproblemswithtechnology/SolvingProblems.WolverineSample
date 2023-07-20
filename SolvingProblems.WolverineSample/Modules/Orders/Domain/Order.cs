using SolvingProblems.WolverineSample.Domain.Abstract;
using SolvingProblems.WolverineSample.Modules.Organizations.Domain;

namespace SolvingProblems.WolverineSample.Modules.Orders.Domain;

public record OrderId : GuidEntityId
{
    public OrderId(Guid id) : base(id) { }
}

/// <summary>
/// Represents an order (TravelPlan purchase and assignation to a Sim)
/// </summary>
public class Order : AggregateRoot<OrderId>
{
    /// <summary>
    /// If this becomes a more complex state machine, 
    /// consider to use stateless nuget + event sourcing
    /// </summary>
    public enum Status { Created = 0, Purchased = 20, Paid = 30 }

    /// <summary>
    /// External ThirdParty reference, such as eSIM Go
    /// </summary>
    public string? ExternalOrderReference { get; private set; }
    public Status OrderStatus { get; private set; }

    /// <summary>
    /// Organization. No hard relation between modules.
    /// </summary>
    public virtual OrganizationId OrganizationId { get; private set; } = null!;

    /// <summary>
    /// Purchaser User. No hard relation between modules.
    /// </summary>
    public virtual UserId UserId { get; private set; } = null!;
    public virtual Guid? PaymentsId { get; private set; } = null!;
    public decimal Price { get; internal set; }

    /// <summary>
    /// Used Sim. No hard relation between modules.
    /// </summary>

    /// <summary>
    /// For EntityFramework
    /// </summary>
    /// <param name="Id"></param>
    private Order(OrderId Id) : base(Id) { }

    /// <summary>
    /// Not meant to be used outside the Domain. Use IOrderCreationService instead.
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="user"></param>
    /// <param name="travelPlan"></param>
    /// <returns></returns>
    public static Order CreateOrder(OrderId orderId, User user, decimal price)
    {
        Order order = new Order(orderId)
        {
            OrderStatus = Status.Created,
            UserId = user.Id,
            OrganizationId = user.OrganizationId,
            Price = price
        };

        order.AddDomainEvent(new OrderCreated(orderId));

        return order;
    }

    public void Purchase(string externalThirdPartyReference)
    {
        if (string.IsNullOrWhiteSpace(externalThirdPartyReference))
            throw new ArgumentNullException(nameof(externalThirdPartyReference), "Should have a valid externalThirdPartyReference");

        if (OrderStatus != Status.Created)
            throw new ArgumentException("The order should be in Purchasing state");

        this.OrderStatus = Status.Purchased;
        this.ExternalOrderReference = externalThirdPartyReference;

        this.AddDomainEvent(new OrderPurchased(Id, externalThirdPartyReference));
    }

    public void Pay(Guid paymentId)
    {
        if (OrderStatus != Status.Purchased)
            throw new ArgumentException("The order should be in Purchased state");

        this.PaymentsId = paymentId;
        this.OrderStatus = Status.Paid;

        this.AddDomainEvent(new OrderPaid(Id));
    }
}