using DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Contracts;
using DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns.StrategyAdvanced;

/// <summary>
/// Dependency injection extension methods for Strategy Advanced pattern.
/// Strategy with Resolver/Factory Pattern - Dynamic provider selection at runtime.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers all services required for the advanced strategy pattern with dynamic provider selection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddStrategyAdvancedPattern(this IServiceCollection services)
    {
        // Register all payment providers as Singleton (they are stateless, safe for singleton)
        // Open/Closed Principle: To add a new provider, just register it here - no other code changes needed
        services.AddSingleton<IPaymentProvider, StripePaymentProvider>();
        services.AddSingleton<IPaymentProvider, PayPalPaymentProvider>();
        services.AddSingleton<IPaymentProvider, CryptoPaymentProvider>();
        
        // Register resolver as Singleton (can safely consume singleton providers)
        // The resolver builds a dictionary of provider keys to types for O(1) lookup
        services.AddSingleton<IPaymentProviderResolver, PaymentProviderResolver>();
        
        // Register payment service as Scoped (per-request instance)
        services.AddScoped<IPaymentService, PaymentService>();
        
        return services;
    }
}
