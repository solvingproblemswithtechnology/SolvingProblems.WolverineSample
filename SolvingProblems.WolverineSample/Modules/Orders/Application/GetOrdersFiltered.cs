using Microsoft.EntityFrameworkCore;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Infrastructure.Data;
using SolvingProblems.WolverineSample.Modules.Orders.Application.Dtos;
using SolvingProblems.WolverineSample.Modules.Orders.Domain;
using SolvingProblems.WolverineSample.Modules.Organizations.Domain;

namespace SolvingProblems.WolverineSample.Modules.Orders.Application;

public static class GetOrdersFiltered
{
    public record Query(Guid? UserId) : IQuery<Response>;

    public class Response
    {
        public IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();

        public Response(IEnumerable<Order> orders)
            => this.Orders = orders.Select(OrderDto.FromOrder).ToList();
    }

    public class Handler : IQueryHandler<Query, Response>
    {
        private readonly BackendDbContext context;

        public Handler(BackendDbContext context) => this.context = context;

        public async Task<Response> Handle(Query request)
        {
            // Should come from the auth token, just for cutting corners
            User loggedUser = await context.Users.FirstAsync();

            IQueryable<Order> query = context.Orders.Where(o => o.OrganizationId == loggedUser.OrganizationId);

            if (request.UserId.HasValue)
            {
                query = context.Orders.Where(o => o.UserId == new UserId(request.UserId.Value));
            }

            List<Order> orders = await query.ToListAsync();
            return new Response(orders);
        }
    }
}
