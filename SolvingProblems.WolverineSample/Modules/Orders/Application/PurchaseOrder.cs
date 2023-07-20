using Microsoft.EntityFrameworkCore;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Infrastructure.Data;
using SolvingProblems.WolverineSample.Modules.Orders.Domain;
using SolvingProblems.WolverineSample.Modules.Organizations.Domain;

namespace SolvingProblems.WolverineSample.Modules.Orders.Application;

public static class PurchaseOrder
{
    public record Command(Guid OrderId, decimal Price) : ICommand;

    public class Handler : ICommandHandler<Command>
    {
        private readonly BackendDbContext context;

        public Handler(BackendDbContext context) => this.context = context;

        public async Task Handle(Command command)
        {
            // Should come from the auth token, just for cutting corners
            User loggedUser = await context.Users.FirstAsync();

            Order? order = await context.Orders
                .FirstOrDefaultAsync(o => o.Id == new OrderId(command.OrderId) && o.OrganizationId == loggedUser.OrganizationId);

            if (order is null)
            {
                order = Order.CreateOrder(new OrderId(command.OrderId), loggedUser, command.Price);
                context.Orders.Add(order);
            }

            var thirdParty = "SomeThirdParyCall";
            order.Purchase(thirdParty);

            await context.SaveEntitiesAsync();
        }
    }
}