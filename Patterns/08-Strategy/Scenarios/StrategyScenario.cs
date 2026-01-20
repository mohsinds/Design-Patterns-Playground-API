using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._08_Strategy.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._08_Strategy.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Strategy pattern.
/// </summary>
public class StrategyScenario
{
    private readonly IPricingStrategySelector _selector;
    private readonly IEnumerable<IPricingStrategy> _strategies;
    private readonly ILogger<StrategyScenario> _logger;
    
    public StrategyScenario(
        IPricingStrategySelector selector,
        IEnumerable<IPricingStrategy> strategies,
        ILogger<StrategyScenario> logger)
    {
        _selector = selector;
        _strategies = strategies;
        _logger = logger;
    }
    
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        var marketQuote = new Quote("AAPL", 150m, 150.5m, 150.25m, DateTime.UtcNow);
        
        // Test different strategies
        foreach (var strategy in _strategies)
        {
            var order = new Order(
                OrderId: $"ORD-{strategy.StrategyName}",
                AccountId: "ACC-001",
                Symbol: "AAPL",
                Side: OrderSide.Buy,
                Quantity: 100,
                Price: 150m,
                Status: OrderStatus.Pending,
                CreatedAt: DateTime.UtcNow
            );
            
            var price = strategy.CalculatePrice(order, marketQuote);
            results.Add(new
            {
                Strategy = strategy.StrategyName,
                OrderValue = order.Quantity * order.Price,
                CalculatedPrice = price,
                MarketBid = marketQuote.Bid,
                MarketAsk = marketQuote.Ask
            });
        }
        
        // Test strategy selection
        var largeOrder = new Order(
            OrderId: "ORD-LARGE",
            AccountId: "ACC-001",
            Symbol: "MSFT",
            Side: OrderSide.Buy,
            Quantity: 10000,
            Price: 300m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var selectedStrategy = _selector.SelectStrategy(largeOrder);
        results.Add(new
        {
            Selection = "Strategy Selection",
            OrderValue = largeOrder.Quantity * largeOrder.Price,
            SelectedStrategy = selectedStrategy.StrategyName
        });
        
        return new PatternDemoResponse(
            Pattern: "Strategy",
            Description: "Demonstrates strategy pattern: different pricing algorithms selected at runtime based on order characteristics.",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["StrategyCount"] = _strategies.Count(),
                ["RuntimeSelection"] = true
            }
        );
    }
    
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        var marketQuote = new Quote("TEST", 100m, 100.5m, 100.25m, DateTime.UtcNow);
        
        // Test 1: All strategies calculate prices
        foreach (var strategy in _strategies)
        {
            var order = new Order(
                OrderId: "ORD-TEST",
                AccountId: "ACC-TEST",
                Symbol: "TEST",
                Side: OrderSide.Buy,
                Quantity: 10,
                Price: 100m,
                Status: OrderStatus.Pending,
                CreatedAt: DateTime.UtcNow
            );
            
            var price = strategy.CalculatePrice(order, marketQuote);
            checks.Add(new TestCheck(
                $"{strategy.StrategyName} Calculates Price",
                price > 0,
                $"{strategy.StrategyName} calculated price: {price}"
            ));
        }
        
        // Test 2: Strategy selector works
        var testOrder = new Order(
            OrderId: "ORD-TEST",
            AccountId: "ACC-TEST",
            Symbol: "TEST",
            Side: OrderSide.Buy,
            Quantity: 10,
            Price: 100m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var selected = _selector.SelectStrategy(testOrder);
        checks.Add(new TestCheck(
            "Strategy Selection",
            selected != null,
            $"Selected strategy: {selected.StrategyName}"
        ));
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Strategy",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
