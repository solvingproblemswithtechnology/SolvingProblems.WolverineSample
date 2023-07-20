using Microsoft.Extensions.Options;

namespace SolvingProblems.WolverineSample.Infrastructure.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static T AddTypedOptions<T>(this WebApplicationBuilder builder, string? sectionName = null)
        where T : class
    {
        sectionName ??= typeof(T).Name;

        IConfigurationSection section = builder.Configuration.GetRequiredSection(sectionName);
        T options = section.Get<T>() ?? throw new ArgumentNullException($"Section {sectionName} can't be parsed to type {typeof(T).Name}");

        builder.Services.AddOptions()
            .Configure<T>(section)
            .AddScoped(provider => provider.GetRequiredService<IOptionsSnapshot<T>>().Value);

        return options;
    }
}