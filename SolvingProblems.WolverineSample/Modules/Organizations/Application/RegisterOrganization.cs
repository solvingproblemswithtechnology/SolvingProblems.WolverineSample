using Microsoft.EntityFrameworkCore;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Infrastructure.Data;
using SolvingProblems.WolverineSample.Modules.Organizations.Domain;
using SolvingProblems.WolverineSample.Modules.Shared.Domain;

namespace SolvingProblems.WolverineSample.Modules.Organizations.Application;

public static class RegisterOrganization
{
    public record Command(Guid OrganizationId, string Name, Guid UserId, string FirstName, string LastName, string Email, string PhoneNumber) : ICommand;

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly BackendDbContext context;

        public CommandHandler(BackendDbContext context) => this.context = context;

        public async Task Handle(Command command)
        {
            OrganizationId id = new OrganizationId(command.OrganizationId);

            Email email = Email.From(command.Email);
            PhoneNumber phoneNumber = PhoneNumber.From("", command.PhoneNumber);

            bool exists = await context.Organizations.AnyAsync(o => o.Id == new OrganizationId(id));

            if (exists)
                throw new ArgumentException("Organization already exists");

            Organization organization = Organization.CreateOrganization(new OrganizationId(id), command.Name);

            User user = User.CreateUser(new UserId(command.UserId), command.FirstName, command.LastName, email);
            user.AssignRole("admin");

            organization.AddUser(user);

            await context.Organizations.AddAsync(organization);
            await context.SaveChangesAsync();
        }
    }
}
