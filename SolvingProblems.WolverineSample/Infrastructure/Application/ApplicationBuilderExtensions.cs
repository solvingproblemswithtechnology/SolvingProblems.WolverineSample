namespace SolvingProblems.WolverineSample.Infrastructure.Application;

public static class ApplicationBuilderExtensions
{
    public static void UseApis(this WebApplication app)
    {
        var apisToRegister = app.Services.GetRequiredService<IEnumerable<Api>>();
        foreach (var apiToRegister in apisToRegister)
            apiToRegister.Map(app);
    }
}
