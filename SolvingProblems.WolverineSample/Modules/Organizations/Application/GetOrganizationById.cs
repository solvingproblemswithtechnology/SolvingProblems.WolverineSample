using Optional;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Infrastructure.Data;
using SolvingProblems.WolverineSample.Modules.Organizations.Application.Dtos;
using SolvingProblems.WolverineSample.Modules.Organizations.Domain;

namespace SolvingProblems.WolverineSample.Modules.Organizations.Application;

public static class GetOrganizationById
{
    // Get Organization by Id, similar to GetOrganizations.cs
    public record Query(Guid OrganizationId) : IQuery<Option<Response>>;

    public class Response
    {
        public OrganizationDto Organization { get; set; }
        public Response(Organization organization)
            => this.Organization = OrganizationDto.FromOrganization(organization);
    }

    public class Handler : IQueryHandler<Query, Option<Response>>
    {
        private readonly BackendDbContext context;

        public Handler(BackendDbContext context) => this.context = context;

        public async Task<Option<Response>> Handle(Query request)
        {
            Organization? organization = await context.Organizations.FindAsync(new OrganizationId(request.OrganizationId));
            return organization is not null ? Option.Some(new Response(organization)) : Option.None<Response>();
        }
    }
}
