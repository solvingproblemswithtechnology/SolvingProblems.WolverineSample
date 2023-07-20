using Microsoft.EntityFrameworkCore;
using Optional;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Infrastructure.Data;
using SolvingProblems.WolverineSample.Modules.Orders.Application.Dtos;
using SolvingProblems.WolverineSample.Modules.Orders.Domain;
using SolvingProblems.WolverineSample.Modules.Organizations.Domain;

namespace SolvingProblems.WolverineSample.Modules.Orders.Application;

public static class GetOrder
{
    // GetOrder.Query based of the Get Order endpoint
    public record Query(Guid OrderId) : IQuery<Option<Response>>;
    public class Response
    {
        public OrderDto Order { get; set; }

        public Response(Order order)
            => this.Order = OrderDto.FromOrder(order);
    }

    public class Handler : IQueryHandler<Query, Option<Response>>
    {
        private readonly BackendDbContext context;

        public Handler(BackendDbContext context) => this.context = context;

        public async Task<Option<Response>> Handle(Query request)
        {
            // Should come from the auth token, just for cutting corners
            User loggedUser = await context.Users.FirstAsync();

            Order? order = await context.Orders
                .FirstOrDefaultAsync(o => o.Id == new OrderId(request.OrderId) && o.OrganizationId == loggedUser.OrganizationId);

            return order is null ? Option.None<Response>() : Option.Some(new Response(order));
        }
    }
}