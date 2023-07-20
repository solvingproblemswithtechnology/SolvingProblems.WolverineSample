using SolvingProblems.WolverineSample.Infrastructure.Application.Events;
using SolvingProblems.WolverineSample.Infrastructure.Data;
using SolvingProblems.WolverineSample.Modules.Orders.Domain;

namespace SolvingProblems.WolverineSample.Modules.Orders.Application.EventHandlers;

public class SendEmailWhenOrderPurchased : IDomainEventHandler<OrderPurchased>
{
    private readonly BackendDbContext context;
    private readonly IHttpContextAccessor accessor;
    private readonly IIntegrationEventBus bus;
    private readonly ILogger<SendEmailWhenOrderPurchased> logger;

    public SendEmailWhenOrderPurchased(BackendDbContext context, IHttpContextAccessor accessor, IIntegrationEventBus bus, ILogger<SendEmailWhenOrderPurchased> logger)
    {
        this.context = context;
        this.accessor = accessor;
        this.bus = bus;
        this.logger = logger;
    }

    public async ValueTask Handle(OrderPurchased e)
    {
        // Find will use the context tracking to get it instead of fetching if already loaded. It can't be null.
        Order order = (await context.Orders.FindAsync(e.OrderId))!;

        var request = accessor.HttpContext!.Request;
        string url = $"{request.Scheme}://{request.Host}";
        this.logger.LogInformation("Logging this here as it would be required for sending emails. {url}", url);

        // We can trigger emails for example from here. We could also send a Command, but it's preferable to be Event Driven and use topics here for lower coupling.
        await bus.EmitAsync(new OrderPurchasedIntegrationEvent(order.Id, order.ExternalOrderReference, order.Price));
    }
}

/// <summary>
/// It could be in a separete file, folder or assebly to share it, but not truly recommended. At most, an spec file.
/// </summary>
/// <param name="OrderId"></param>
/// <param name="externalOrderReference"></param>
/// <param name="price"></param>
public record OrderPurchasedIntegrationEvent(Guid OrderId, string? externalOrderReference, decimal price) : IIntegrationEvent;