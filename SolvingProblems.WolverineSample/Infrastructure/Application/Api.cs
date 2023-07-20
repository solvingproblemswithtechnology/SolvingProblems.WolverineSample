namespace SolvingProblems.WolverineSample.Infrastructure.Application;

public abstract class Api
{
    public abstract IServiceCollection ConfigureServices(IServiceCollection services);

    public abstract void Map(IEndpointRouteBuilder builder);
}
