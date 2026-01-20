using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._08_Strategy.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._08_Strategy.Implementations;

/// <summary>
/// Market price strategy: uses current market price.
/// </summary>
public class MarketPriceStrategy : IPricingStrategy
{
    public string StrategyName => "MarketPrice";
    
    public decimal CalculatePrice(Order order, Quote marketQuote)
    {
        return order.Side == OrderSide.Buy ? marketQuote.Ask : marketQuote.Bid;
    }
}

/// <summary>
/// Limit price strategy: uses order's limit price if set, otherwise market price.
/// </summary>
public class LimitPriceStrategy : IPricingStrategy
{
    public string StrategyName => "LimitPrice";
    
    public decimal CalculatePrice(Order order, Quote marketQuote)
    {
        // If order has a limit price, use it; otherwise use market price
        if (order.Price > 0 && order.Price != marketQuote.Last)
            return order.Price;
        
        return order.Side == OrderSide.Buy ? marketQuote.Ask : marketQuote.Bid;
    }
}

/// <summary>
/// VWAP (Volume Weighted Average Price) strategy: calculates based on volume.
/// </summary>
public class VwapPricingStrategy : IPricingStrategy
{
    public string StrategyName => "VWAP";
    
    public decimal CalculatePrice(Order order, Quote marketQuote)
    {
        // Simplified VWAP calculation
        var midPrice = (marketQuote.Bid + marketQuote.Ask) / 2;
        var spread = marketQuote.Ask - marketQuote.Bid;
        
        // Adjust based on order size (larger orders get better price)
        var sizeAdjustment = order.Quantity > 1000 ? -0.001m : 0m;
        
        return midPrice + (spread * 0.1m) + sizeAdjustment;
    }
}

/// <summary>
/// Risk-adjusted pricing strategy: adds risk premium.
/// </summary>
public class RiskAdjustedPricingStrategy : IPricingStrategy
{
    private readonly decimal _riskPremium;
    
    public RiskAdjustedPricingStrategy(decimal riskPremium = 0.02m)
    {
        _riskPremium = riskPremium;
    }
    
    public string StrategyName => "RiskAdjusted";
    
    public decimal CalculatePrice(Order order, Quote marketQuote)
    {
        var basePrice = order.Side == OrderSide.Buy ? marketQuote.Ask : marketQuote.Bid;
        
        // Add risk premium (higher for larger orders)
        var orderValue = order.Quantity * basePrice;
        var riskMultiplier = orderValue > 100000m ? 1.5m : 1.0m;
        
        return basePrice * (1 + (_riskPremium * riskMultiplier));
    }
}
