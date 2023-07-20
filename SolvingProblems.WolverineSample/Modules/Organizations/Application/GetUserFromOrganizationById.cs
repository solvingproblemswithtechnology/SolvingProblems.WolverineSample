using Microsoft.EntityFrameworkCore;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Infrastructure.Data;
using SolvingProblems.WolverineSample.Modules.Organizations.Application.Dtos;
using SolvingProblems.WolverineSample.Modules.Organizations.Domain;

namespace SolvingProblems.WolverineSample.Modules.Organizations.Application;

public class GetUserFromOrganizationById
{
    // Query getting an user of an organization by its id
    public record Query(Guid OrganizationId, Guid UserId) : IQuery<Response>;
    public class Response
    {
        public UserDto User { get; set; }

        public Response(User user)
            => this.User = UserDto.FromUser(user);
    }

    public class QueryHandler : IQueryHandler<Query, Response>
    {
        private readonly BackendDbContext context;

        public QueryHandler(BackendDbContext context) => this.context = context;

        public async Task<Response> Handle(Query command)
        {
            OrganizationId organizationId = new OrganizationId(command.OrganizationId);
            UserId userId = new UserId(command.UserId);

            User? user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == userId
                    && u.OrganizationId == organizationId);

            if (user is null)
                throw new ArgumentNullException("User doesn't exists");

            return new Response(user);
        }
    }
}