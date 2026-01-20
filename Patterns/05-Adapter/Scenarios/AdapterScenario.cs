using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._05_Adapter.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._05_Adapter.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Adapter pattern.
/// </summary>
public class AdapterScenario
{
    private readonly IModernMarketDataProvider _modernProvider;
    private readonly ILogger<AdapterScenario> _logger;
    
    public AdapterScenario(
        IModernMarketDataProvider modernProvider,
        ILogger<AdapterScenario> logger)
    {
        _modernProvider = modernProvider;
        _logger = logger;
    }
    
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Get quotes using modern interface (which adapts legacy provider)
        var symbols = new[] { "AAPL", "MSFT", "GOOGL" };
        
        foreach (var symbol in symbols)
        {
            var quote = _modernProvider.GetQuoteAsync(symbol).Result;
            results.Add(new
            {
                Symbol = symbol,
                Quote = quote,
                Spread = quote.Ask - quote.Bid
            });
        }
        
        return new PatternDemoResponse(
            Pattern: "Adapter",
            Description: "Demonstrates adapter pattern: legacy market data provider adapted to modern async interface.",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["LegacyToModern"] = true,
                ["AsyncAdaptation"] = true
            }
        );
    }
    
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Test 1: Adapter returns valid quote
        var quote = _modernProvider.GetQuoteAsync("TEST").Result;
        checks.Add(new TestCheck(
            "Adapter Returns Quote",
            quote != null && quote.Symbol == "TEST",
            $"Retrieved quote for {quote.Symbol}"
        ));
        
        // Test 2: Quote has valid values
        checks.Add(new TestCheck(
            "Quote Values Valid",
            quote.Bid > 0 && quote.Ask > quote.Bid,
            $"Bid={quote.Bid}, Ask={quote.Ask}, Spread={quote.Ask - quote.Bid}"
        ));
        
        // Test 3: Timestamp is recent
        var timeDiff = DateTime.UtcNow - quote.Timestamp;
        checks.Add(new TestCheck(
            "Quote Timestamp Recent",
            timeDiff.TotalSeconds < 5,
            $"Timestamp is {timeDiff.TotalSeconds:F2} seconds ago"
        ));
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Adapter",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
