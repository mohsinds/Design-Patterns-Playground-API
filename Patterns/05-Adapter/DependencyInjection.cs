using DesignPatterns.Playground.Api.Patterns._05_Adapter.Contracts;
using DesignPatterns.Playground.Api.Patterns._05_Adapter.Implementations;
using DesignPatterns.Playground.Api.Patterns._05_Adapter.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._05_Adapter;

public static class DependencyInjection
{
    public static IServiceCollection AddAdapterPattern(this IServiceCollection services)
    {
        services.AddSingleton<ILegacyMarketDataProvider, LegacyMarketDataProvider>();
        services.AddScoped<IModernMarketDataProvider, MarketDataAdapter>();
        services.AddScoped<AdapterScenario>();
        
        return services;
    }
}
