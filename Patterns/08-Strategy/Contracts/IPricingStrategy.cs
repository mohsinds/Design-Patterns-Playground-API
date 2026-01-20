using DesignPatterns.Playground.Api.Domain;

namespace DesignPatterns.Playground.Api.Patterns._08_Strategy.Contracts;

/// <summary>
/// Pricing strategy interface.
/// Strategy pattern: allows selecting algorithm at runtime (routing, risk, pricing).
/// </summary>
public interface IPricingStrategy
{
    /// <summary>
    /// Calculate price for an order.
    /// </summary>
    decimal CalculatePrice(Order order, Quote marketQuote);
    
    /// <summary>
    /// Get strategy name.
    /// </summary>
    string StrategyName { get; }
}
