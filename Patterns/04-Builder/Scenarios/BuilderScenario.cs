using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._04_Builder.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._04_Builder.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Builder pattern.
/// </summary>
public class BuilderScenario
{
    private readonly IOrderBuilder _builder;
    private readonly ILogger<BuilderScenario> _logger;
    
    public BuilderScenario(
        IOrderBuilder builder,
        ILogger<BuilderScenario> logger)
    {
        _builder = builder;
        _logger = logger;
    }
    
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Build a simple order
        var simpleOrder = _builder
            .Reset()
            .WithAccount("ACC-001")
            .WithSymbol("AAPL")
            .WithSide(OrderSide.Buy)
            .WithQuantity(100)
            .WithPrice(150m)
            .Build();
        
        results.Add(new
        {
            Type = "Simple Order",
            Order = simpleOrder
        });
        
        // Build a complex order with limit price
        var complexOrder = _builder
            .Reset()
            .WithAccount("ACC-002")
            .WithSymbol("MSFT")
            .WithSide(OrderSide.Sell)
            .WithQuantity(500)
            .WithPrice(300m)
            .WithLimitPrice(305m)
            .Build();
        
        results.Add(new
        {
            Type = "Complex Order",
            Order = complexOrder
        });
        
        return new PatternDemoResponse(
            Pattern: "Builder",
            Description: "Demonstrates builder pattern: fluent interface for constructing complex Order objects step-by-step.",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["FluentInterface"] = true,
                ["Validation"] = "Builder validates required fields before building"
            }
        );
    }
    
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Test 1: Builder creates valid order
        var order = _builder
            .Reset()
            .WithAccount("ACC-TEST")
            .WithSymbol("TEST")
            .WithSide(OrderSide.Buy)
            .WithQuantity(10)
            .WithPrice(100m)
            .Build();
        
        checks.Add(new TestCheck(
            "Builder Creates Valid Order",
            order != null && order.OrderId != null,
            $"Created order {order.OrderId}"
        ));
        
        // Test 2: Order has correct values
        checks.Add(new TestCheck(
            "Order Values Correct",
            order.AccountId == "ACC-TEST" && order.Symbol == "TEST" && order.Quantity == 10,
            $"Order values match: Account={order.AccountId}, Symbol={order.Symbol}, Quantity={order.Quantity}"
        ));
        
        // Test 3: Builder throws on missing required field
        try
        {
            _builder.Reset().WithAccount("ACC-TEST").Build();
            checks.Add(new TestCheck(
                "Builder Validation",
                false,
                "Builder should throw on missing required fields"
            ));
        }
        catch (InvalidOperationException)
        {
            checks.Add(new TestCheck(
                "Builder Validation",
                true,
                "Builder correctly validates required fields"
            ));
        }
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Builder",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
