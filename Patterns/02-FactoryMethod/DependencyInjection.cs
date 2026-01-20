using DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Contracts;
using DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Implementations;
using DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._02_FactoryMethod;

/// <summary>
/// Dependency injection extension for Factory Method pattern.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddFactoryMethodPattern(this IServiceCollection services)
    {
        services.AddScoped<IOrderValidatorFactory, OrderValidatorFactory>();
        services.AddScoped<FactoryMethodScenario>();
        
        return services;
    }
}
