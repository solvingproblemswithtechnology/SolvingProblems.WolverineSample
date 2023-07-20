using SolvingProblems.WolverineSample.Infrastructure.Application.Events;
using SolvingProblems.WolverineSample.Infrastructure.Data;
using SolvingProblems.WolverineSample.Modules.Orders.Domain;

namespace SolvingProblems.WolverineSample.Modules.Orders.Application.EventHandlers;


/// <summary>
/// It could be in a separete file, folder or assebly to share it, but not truly recommended. 
/// At most, an spec file. Probably a version is needed in the Envelop level.
/// </summary>
/// <param name="OrderId"></param>
/// <param name="externalOrderReference"></param>
/// <param name="price"></param>
public record PaymentProcessedIntegrationEvent(Guid OrderId, Guid PaymentId) : IIntegrationEvent;

public class PayOrderWhenPaymentProcessed : IIntegrationEventHandler<PaymentProcessedIntegrationEvent>
{
    private readonly BackendDbContext context;

    public PayOrderWhenPaymentProcessed(BackendDbContext context) => this.context = context;

    public async ValueTask Handle(PaymentProcessedIntegrationEvent request)
    {
        Order? order = await context.Orders.FindAsync(request.OrderId);

        // Shouldn't happen, but can happen with eventual consistency. An error while be just retried at subscription level.
        if (order is null)
            throw new ArgumentException($"Order with id {request.OrderId} not found");

        order.Pay(request.PaymentId);

        await context.SaveEntitiesAsync();
    }
}
