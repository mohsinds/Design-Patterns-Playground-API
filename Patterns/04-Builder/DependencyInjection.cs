using DesignPatterns.Playground.Api.Patterns._04_Builder.Contracts;
using DesignPatterns.Playground.Api.Patterns._04_Builder.Implementations;
using DesignPatterns.Playground.Api.Patterns._04_Builder.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._04_Builder;

public static class DependencyInjection
{
    public static IServiceCollection AddBuilderPattern(this IServiceCollection services)
    {
        services.AddScoped<IOrderBuilder, OrderBuilder>();
        services.AddScoped<BuilderScenario>();
        
        return services;
    }
}
