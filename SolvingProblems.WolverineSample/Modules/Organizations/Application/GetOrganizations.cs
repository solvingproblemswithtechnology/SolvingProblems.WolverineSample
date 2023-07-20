using Microsoft.EntityFrameworkCore;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Infrastructure.Data;
using SolvingProblems.WolverineSample.Modules.Organizations.Application.Dtos;
using SolvingProblems.WolverineSample.Modules.Organizations.Domain;

namespace SolvingProblems.WolverineSample.Modules.Organizations.Application;

public static class GetOrganizations
{
    // record query y result. class handler
    public record Query : IQuery<Response>;

    public class Response
    {
        public IEnumerable<OrganizationDto> Organizations { get; set; }

        public Response(IEnumerable<Organization> organizations)
            => this.Organizations = organizations.Select(OrganizationDto.FromOrganization).ToList();
    }

    public class Handler : IQueryHandler<Query, Response>
    {
        private readonly BackendDbContext context;

        public Handler(BackendDbContext context) => this.context = context;

        public async Task<Response> Handle(Query request)
        {
            List<Organization> organizations = await context.Organizations.ToListAsync();
            return new Response(organizations);
        }
    }
}
