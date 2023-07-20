using SolvingProblems.WolverineSample.Modules.Orders.Domain;

namespace SolvingProblems.WolverineSample.Modules.Orders.Application.Dtos;

public record OrderDto(Guid OrderId, string OrderStatus, Guid OrganizationId, Guid UserId, string? ExternalOrderReference)
{
    public static OrderDto FromOrder(Order order) => new OrderDto(order.Id.AsGuid(), order.OrderStatus.ToString(),
        order.OrganizationId.AsGuid(), order.UserId.AsGuid(), order.ExternalOrderReference);
}