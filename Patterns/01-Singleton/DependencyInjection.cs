using DesignPatterns.Playground.Api.Patterns._01_Singleton.Contracts;
using DesignPatterns.Playground.Api.Patterns._01_Singleton.Implementations;
using DesignPatterns.Playground.Api.Patterns._01_Singleton.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._01_Singleton;

/// <summary>
/// Dependency injection extension for Singleton pattern.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Register Singleton pattern services.
    /// Note: AddSingleton ensures one instance per application instance (not across pods in Kubernetes).
    /// </summary>
    public static IServiceCollection AddSingletonPattern(this IServiceCollection services)
    {
        // Register as singleton - one instance per application instance
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddScoped<SingletonScenario>();
        
        return services;
    }
}
