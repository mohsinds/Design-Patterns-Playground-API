using DesignPatterns.Playground.Api.Domain;

namespace DesignPatterns.Playground.Api.Patterns._08_Strategy.Contracts;

/// <summary>
/// Strategy selector interface.
/// </summary>
public interface IPricingStrategySelector
{
    /// <summary>
    /// Select appropriate pricing strategy based on order characteristics.
    /// </summary>
    IPricingStrategy SelectStrategy(Order order);
}
