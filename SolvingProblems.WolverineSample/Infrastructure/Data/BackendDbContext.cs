using Microsoft.EntityFrameworkCore;
using SolvingProblems.WolverineSample.Infrastructure.Data.Shared;
using SolvingProblems.WolverineSample.Modules.Orders.Domain;
using SolvingProblems.WolverineSample.Modules.Organizations.Domain;

namespace SolvingProblems.WolverineSample.Infrastructure.Data;

public class BackendDbContext : SmartDbContext
{
    public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options) { }

    #region Entities

    /* Organizations Module */
    public DbSet<Organization> Organizations { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    /* Orders Module */
    public DbSet<Order> Orders { get; set; } = null!;

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO If this gets bigger, split into Entity Builders in each Module
        modelBuilder.Entity<Organization>(e =>
        {
            e.HasKey(o => o.Id);
            e.OwnsOne(o => o.TaxEmail);

            e.Property(o => o.OrganizationStatus).HasDefaultValue(Organization.Status.Created);
        });
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.OwnsOne(u => u.Email);
        });

        modelBuilder.Entity<Order>(e => e.HasKey(o => o.Id));

        base.OnModelCreating(modelBuilder);
    }
}
