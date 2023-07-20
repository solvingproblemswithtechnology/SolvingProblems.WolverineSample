using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SolvingProblems.WolverineSample.Infrastructure.Data.Shared;

public static class EntityFrameworkServiceCollectionExtensions
{
    public static IServiceCollection AddSmartDbContext<T>(this IServiceCollection services, string connectionString, string? schema = null)
        where T : SmartDbContext
    {
        return services.AddDbContextPool<T>((provider, options) =>
        {
            options.ReplaceService<IValueConverterSelector, SmartValueConverter>();
            options.UseSqlServer(connectionString, c =>
            {
                //c.EnableRetryOnFailure(5);
                c.MinBatchSize(2).MaxBatchSize(50);

                if (schema is not null)
                    c.MigrationsHistoryTable("__EFMigrationsHistory", schema);
            });
        }).AddScoped(provider =>
        {
#pragma warning disable EF1001 // Internal EF Core API usage.
            var context = provider.GetRequiredService<IScopedDbContextLease<T>>().Context;
#pragma warning restore EF1001 // Internal EF Core API usage.
            context.SetServiceProvider(provider); // 
            return context;
        });
    }
}
