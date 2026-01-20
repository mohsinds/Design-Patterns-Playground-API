namespace DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Contracts;

/// <summary>
/// Resolver interface for dynamically selecting payment providers at runtime.
/// Strategy pattern with Resolver/Factory: Enables runtime selection of strategies based on data.
/// </summary>
public interface IPaymentProviderResolver
{
    /// <summary>
    /// Resolves a payment provider by its unique key.
    /// </summary>
    /// <param name="providerKey">The unique key identifier of the payment provider.</param>
    /// <returns>The payment provider if found; otherwise, null.</returns>
    IPaymentProvider? ResolveProvider(string providerKey);
    
    /// <summary>
    /// Gets all available payment providers.
    /// </summary>
    /// <returns>A collection of all registered payment providers.</returns>
    IEnumerable<IPaymentProvider> GetAvailableProviders();
}
