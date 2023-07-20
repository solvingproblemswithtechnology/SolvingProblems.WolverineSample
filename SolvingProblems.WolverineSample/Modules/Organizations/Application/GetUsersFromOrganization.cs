using Microsoft.EntityFrameworkCore;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Infrastructure.Data;
using SolvingProblems.WolverineSample.Modules.Organizations.Application.Dtos;
using SolvingProblems.WolverineSample.Modules.Organizations.Domain;

namespace SolvingProblems.WolverineSample.Modules.Organizations.Application;

public static class GetUsersFromOrganization
{
    public record Query(Guid OrganizationId) : IQuery<Response>;

    public class Response
    {
        public IEnumerable<UserDto> Users { get; set; }

        public Response(IEnumerable<User> users)
            => this.Users = users.Select(UserDto.FromUser).ToList();
    }

    public class QueryHandler : IQueryHandler<Query, Response>
    {
        private readonly BackendDbContext context;

        public QueryHandler(BackendDbContext context) => this.context = context;

        public async Task<Response> Handle(Query command)
        {
            OrganizationId id = new OrganizationId(command.OrganizationId);

            var organization = await context.Organizations
                    .Include(o => o.Users)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (organization is null)
                throw new ArgumentNullException("Organization doesn't exists");

            return new Response(organization.Users);
        }
    }
}
