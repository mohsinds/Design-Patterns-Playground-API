using DesignPatterns.Playground.Api.Patterns._10_Facade.Contracts;
using DesignPatterns.Playground.Api.Patterns._10_Facade.Implementations;
using DesignPatterns.Playground.Api.Patterns._10_Facade.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._10_Facade;

public static class DependencyInjection
{
    public static IServiceCollection AddFacadePattern(this IServiceCollection services)
    {
        services.AddScoped<ITradingFacade, TradingFacade>();
        services.AddScoped<FacadeScenario>();
        
        return services;
    }
}
