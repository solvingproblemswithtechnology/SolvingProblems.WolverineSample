using Microsoft.EntityFrameworkCore;
using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Infrastructure.Data;
using SolvingProblems.WolverineSample.Modules.Organizations.Domain;
using SolvingProblems.WolverineSample.Modules.Shared.Domain;

namespace SolvingProblems.WolverineSample.Modules.Organizations.Application;

public static class UpdateUser
{
    // Command which creates or modifies and user like in Update User endpoint
    public record Command(Guid OrganizationId, Guid UserId, string FirstName, string LastName, string Email, string Role, string? PersonalPhoneNumber, string? Position, string? Department) : ICommand;

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly BackendDbContext context;

        public CommandHandler(BackendDbContext context) => this.context = context;

        public async Task Handle(Command command)
        {
            UserId userId = new(command.UserId);
            OrganizationId organizationId = new(command.OrganizationId);
            Email email = Email.From(command.Email);

            Organization? organization = await context.Organizations.FindAsync(organizationId);

            if (organization is null)
                throw new ArgumentNullException("Organization doesn't exists");

            User? user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId
                && u.OrganizationId == organizationId);

            if (user is null)
            {
                user = User.CreateUser(userId, command.FirstName, command.LastName, email);
                organization.AddUser(user);
                context.Users.Add(user);
                context.Organizations.Update(organization);
            }
            else
            {
                user.ModifyMainInformation(command.FirstName, command.LastName, email);
                context.Users.Update(user);
            }

            PhoneNumber? number = command.PersonalPhoneNumber is not null
                ? PhoneNumber.From("", command.PersonalPhoneNumber) : null;

            user.ModifyExtraInformation(number, command.Position, command.Department);
            user.AssignRole(command.Role);

            await context.SaveEntitiesAsync();
        }
    }
}