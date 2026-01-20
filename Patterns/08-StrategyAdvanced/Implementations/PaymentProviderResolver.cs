using DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Contracts;

namespace DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Implementations;

/// <summary>
/// Payment provider resolver implementation.
/// Strategy pattern with Resolver/Factory: Provides O(1) lookup for payment providers using dictionary.
/// 
/// KEY ADVANTAGES:
/// 1. Open/Closed Principle: To add a new provider (e.g., ApplePayProvider), you only need to:
///    - Create new provider class implementing IPaymentProvider
///    - Register it in DependencyInjection.cs
///    - NO changes to existing code (controller, service, resolver)
/// 
/// 2. Runtime Selection: Provider is chosen based on data (providerKey), not compile-time decisions
/// 
/// 3. Testability: Easy to mock IPaymentProviderResolver and IPaymentService
/// 
/// 4. Performance: O(1) provider lookup using dictionary
/// 
/// 5. Discoverability: GetAvailableProviders() dynamically discovers all registered providers
/// 
/// HOW IT WORKS:
/// =============
/// 1. Constructor receives all registered providers via DI (IEnumerable injection)
/// 2. Builds dictionary mapping provider keys to provider instances
/// 3. ResolveProvider() performs O(1) lookup by key
/// 4. GetAvailableProviders() returns all registered providers
/// 5. Providers are Singleton (stateless), so resolver can be Singleton too
/// </summary>
public sealed class PaymentProviderResolver : IPaymentProviderResolver
{
    private readonly IReadOnlyDictionary<string, IPaymentProvider> _providers;
    private readonly ILogger<PaymentProviderResolver> _logger;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentProviderResolver"/> class.
    /// </summary>
    /// <param name="providers">Collection of all registered payment providers.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when providers or logger is null.</exception>
    public PaymentProviderResolver(
        IEnumerable<IPaymentProvider> providers,
        ILogger<PaymentProviderResolver> logger)
    {
        ArgumentNullException.ThrowIfNull(providers);
        ArgumentNullException.ThrowIfNull(logger);
        
        _logger = logger;
        
        // Build dictionary for O(1) lookup performance
        // Thread-safety: Dictionary is built once during construction and never modified
        // Providers are Singleton (stateless), so safe to store references
        _providers = providers.ToDictionary(
            p => p.ProviderKey,
            p => p,
            StringComparer.OrdinalIgnoreCase);
        
        _logger.LogInformation("PaymentProviderResolver initialized with {Count} providers: {Providers}",
            _providers.Count,
            string.Join(", ", _providers.Keys));
    }
    
    /// <inheritdoc />
    public IPaymentProvider? ResolveProvider(string providerKey)
    {
        if (string.IsNullOrWhiteSpace(providerKey))
        {
            _logger.LogWarning("Attempted to resolve provider with null or empty key");
            return null;
        }
        
        _providers.TryGetValue(providerKey, out var provider);
        
        if (provider == null)
        {
            _logger.LogWarning("Payment provider '{ProviderKey}' not found. Available providers: {AvailableProviders}",
                providerKey,
                string.Join(", ", _providers.Keys));
        }
        
        return provider;
    }
    
    /// <inheritdoc />
    public IEnumerable<IPaymentProvider> GetAvailableProviders() => _providers.Values;
}
