using DesignPatterns.Playground.Api.Patterns._14_Prototype.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._14_Prototype;

public static class DependencyInjection
{
    public static IServiceCollection AddPrototypePattern(this IServiceCollection services)
    {
        services.AddScoped<PrototypeScenario>();
        
        return services;
    }
}
