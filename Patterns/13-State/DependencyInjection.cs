using DesignPatterns.Playground.Api.Patterns._13_State.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._13_State;

public static class DependencyInjection
{
    public static IServiceCollection AddStatePattern(this IServiceCollection services)
    {
        services.AddScoped<StateScenario>();
        
        return services;
    }
}
