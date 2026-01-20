using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._15_ChainOfResponsibility.Contracts;
using DesignPatterns.Playground.Api.Patterns._15_ChainOfResponsibility.Implementations;

namespace DesignPatterns.Playground.Api.Patterns._15_ChainOfResponsibility.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Chain of Responsibility pattern.
/// </summary>
public class ChainOfResponsibilityScenario
{
    private readonly IValidationHandler _validationChain;
    private readonly ILogger<ChainOfResponsibilityScenario> _logger;
    
    public ChainOfResponsibilityScenario(
        IValidationHandler validationChain,
        ILogger<ChainOfResponsibilityScenario> logger)
    {
        _validationChain = validationChain;
        _logger = logger;
    }
    
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Valid order
        var validOrder = new Order(
            OrderId: "ORD-CHAIN-001",
            AccountId: "ACC-001",
            Symbol: "AAPL",
            Side: OrderSide.Buy,
            Quantity: 100,
            Price: 150m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var validResult = _validationChain.HandleAsync(validOrder).Result;
        results.Add(new
        {
            Order = "Valid Order",
            Result = validResult
        });
        
        // Invalid order (fails basic validation)
        var invalidOrder = new Order(
            OrderId: "ORD-CHAIN-002",
            AccountId: "ACC-001",
            Symbol: "",
            Side: OrderSide.Buy,
            Quantity: -10,
            Price: 150m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var invalidResult = _validationChain.HandleAsync(invalidOrder).Result;
        results.Add(new
        {
            Order = "Invalid Order (Basic Validation)",
            Result = invalidResult
        });
        
        // Order that fails risk validation
        var riskOrder = new Order(
            OrderId: "ORD-CHAIN-003",
            AccountId: "ACC-001",
            Symbol: "MSFT",
            Side: OrderSide.Buy,
            Quantity: 10000,
            Price: 300m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var riskResult = _validationChain.HandleAsync(riskOrder).Result;
        results.Add(new
        {
            Order = "Risk Validation",
            OrderValue = riskOrder.Quantity * riskOrder.Price,
            Result = riskResult
        });
        
        return new PatternDemoResponse(
            Pattern: "Chain of Responsibility",
            Description: "Demonstrates chain of responsibility pattern: validation pipeline where each handler processes or passes to next.",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["ChainOrder"] = "Basic -> Risk -> Account",
                ["Flexibility"] = "Easy to add/remove/reorder handlers"
            }
        );
    }
    
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Test 1: Valid order passes all handlers
        var validOrder = new Order(
            OrderId: "ORD-TEST",
            AccountId: "ACC-001",
            Symbol: "TEST",
            Side: OrderSide.Buy,
            Quantity: 10,
            Price: 100m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var validResult = _validationChain.HandleAsync(validOrder).Result;
        checks.Add(new TestCheck(
            "Valid Order Passes",
            validResult.IsValid,
            "Valid order passed all validation handlers"
        ));
        
        // Test 2: Invalid order fails at first handler
        var invalidOrder = new Order(
            OrderId: "ORD-TEST",
            AccountId: "ACC-001",
            Symbol: "",
            Side: OrderSide.Buy,
            Quantity: -10,
            Price: 100m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var invalidResult = _validationChain.HandleAsync(invalidOrder).Result;
        checks.Add(new TestCheck(
            "Invalid Order Fails",
            !invalidResult.IsValid && invalidResult.Errors.Count > 0,
            $"Validation failed with errors: {string.Join(", ", invalidResult.Errors)}"
        ));
        
        // Test 3: Chain processes in order
        var riskOrder = new Order(
            OrderId: "ORD-TEST",
            AccountId: "ACC-001",
            Symbol: "TEST",
            Side: OrderSide.Buy,
            Quantity: 10000,
            Price: 300m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var riskResult = _validationChain.HandleAsync(riskOrder).Result;
        var hasRiskError = riskResult.Errors.Any(e => e.Contains("exceeds maximum"));
        checks.Add(new TestCheck(
            "Chain Processes in Order",
            !riskResult.IsValid && hasRiskError,
            "Risk validation handler caught the error"
        ));
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Chain of Responsibility",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
