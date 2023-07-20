using SolvingProblems.WolverineSample.Infrastructure.Application.Cqrs;
using SolvingProblems.WolverineSample.Infrastructure.Data;
using SolvingProblems.WolverineSample.Modules.Organizations.Domain;
using SolvingProblems.WolverineSample.Modules.Shared.Domain;

namespace SolvingProblems.WolverineSample.Modules.Organizations.Application;

public static class UpdateOrganization
{
    public record Command(Guid OrganizationId, string Name, decimal MonthlyBudgetLimit, string OrganizationStatus, string? TaxId, string? TaxBusinessName, string? TaxFirstName, string? TaxLastName, string? TaxAddress, string? TaxEmail) : ICommand;

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly BackendDbContext context;

        public CommandHandler(BackendDbContext context) => this.context = context;

        public async Task Handle(Command command)
        {
            OrganizationId id = new OrganizationId(command.OrganizationId);

            await context.Database.BeginTransactionAsync();

            Organization? organization = await context.Organizations.FindAsync(id);

            if (organization is null)
                throw new ArgumentNullException("Organization doesn't exists");

            // TODO Validate nulls
            Email email = Email.From(command.TaxEmail!);
            organization.ModifyTaxInformation(command.TaxId!, command.TaxBusinessName!, command.TaxFirstName!, command.TaxLastName!, command.TaxAddress!, email);

            await context.SaveEntitiesAsync();
        }
    }
}