using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._08_Strategy.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._08_Strategy.Implementations;

/// <summary>
/// Strategy selector implementation.
/// Strategy pattern: selects appropriate algorithm based on context.
/// Thread-safety: Stateless selector, safe for concurrent use.
/// 
/// HOW IT WORKS:
/// =============
/// 1. Constructor receives all registered strategies via DI (IEnumerable injection)
/// 2. SelectStrategy() examines order characteristics:
///    - Calculates orderValue = Quantity * Price
///    - Checks conditions in priority order (most specific first)
/// 
/// 3. Selection logic (priority order):
///    a) If orderValue > 500,000: RiskAdjustedPricingStrategy (high-value orders need risk premium)
///    b) Else if order has limit price: LimitPriceStrategy (use order's specified price)
///    c) Else if quantity > 1000: VwapPricingStrategy (large orders use volume-weighted pricing)
///    d) Otherwise: MarketPriceStrategy (default - use current market price)
/// 
/// 4. Uses LINQ First() to find strategy by StrategyName
/// 5. Returns IPricingStrategy interface (polymorphism)
/// 
/// EXTENSIBILITY:
/// - To add new strategy: Register in DI, add condition in SelectStrategy()
/// - Strategies are interchangeable - can swap implementations
/// - Selection logic can be externalized to configuration
/// </summary>
public class PricingStrategySelector : IPricingStrategySelector
{
    private readonly IEnumerable<IPricingStrategy> _strategies;
    
    public PricingStrategySelector(IEnumerable<IPricingStrategy> strategies)
    {
        _strategies = strategies;
    }
    
    public IPricingStrategy SelectStrategy(Order order)
    {
        var orderValue = order.Quantity * order.Price;
        
        // Strategy selection logic
        if (orderValue > 500000m)
            return _strategies.First(s => s.StrategyName == "RiskAdjusted");
        
        if (order.Price > 0 && order.Price != 0)
            return _strategies.First(s => s.StrategyName == "LimitPrice");
        
        if (order.Quantity > 1000)
            return _strategies.First(s => s.StrategyName == "VWAP");
        
        return _strategies.First(s => s.StrategyName == "MarketPrice");
    }
}
