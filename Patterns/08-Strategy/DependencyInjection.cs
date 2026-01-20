using DesignPatterns.Playground.Api.Patterns._08_Strategy.Contracts;
using DesignPatterns.Playground.Api.Patterns._08_Strategy.Implementations;
using DesignPatterns.Playground.Api.Patterns._08_Strategy.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._08_Strategy;

public static class DependencyInjection
{
    public static IServiceCollection AddStrategyPattern(this IServiceCollection services)
    {
        services.AddScoped<IPricingStrategy, MarketPriceStrategy>();
        services.AddScoped<IPricingStrategy, LimitPriceStrategy>();
        services.AddScoped<IPricingStrategy, VwapPricingStrategy>();
        services.AddScoped<IPricingStrategy, RiskAdjustedPricingStrategy>();
        services.AddScoped<IPricingStrategySelector, PricingStrategySelector>();
        services.AddScoped<StrategyScenario>();
        
        return services;
    }
}
