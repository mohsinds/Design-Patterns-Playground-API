using DesignPatterns.Playground.Api.Patterns._05_Adapter.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._05_Adapter.Implementations;

/// <summary>
/// Legacy market data provider implementation (simulates old system).
/// </summary>
public class LegacyMarketDataProvider : ILegacyMarketDataProvider
{
    private static readonly Random _random = new(50);
    
    public (string symbol, double bid, double ask, long timestamp) GetQuoteLegacy(string symbol)
    {
        // Simulate legacy API call
        var basePrice = 100.0 + _random.NextDouble() * 50;
        var bid = basePrice - 0.5;
        var ask = basePrice + 0.5;
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
        return (symbol, bid, ask, timestamp);
    }
}
