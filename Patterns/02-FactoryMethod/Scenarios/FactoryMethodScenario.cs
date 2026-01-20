using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Factory Method pattern.
/// </summary>
public class FactoryMethodScenario
{
    private readonly IOrderValidatorFactory _factory;
    private readonly ILogger<FactoryMethodScenario> _logger;
    
    public FactoryMethodScenario(
        IOrderValidatorFactory factory,
        ILogger<FactoryMethodScenario> logger)
    {
        _factory = factory;
        _logger = logger;
    }
    
    /// <summary>
    /// Demo: Show factory creating different validators based on order characteristics.
    /// </summary>
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Create a standard order
        var standardOrder = new Order(
            OrderId: "ORD-001",
            AccountId: "ACC-001",
            Symbol: "AAPL",
            Side: OrderSide.Buy,
            Quantity: 100,
            Price: 150m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var standardValidator = _factory.CreateValidator(standardOrder);
        var standardResult = standardValidator.Validate(standardOrder);
        results.Add(new
        {
            OrderType = "Standard",
            OrderValue = standardOrder.Quantity * standardOrder.Price,
            ValidatorType = standardValidator.ValidatorType,
            IsValid = standardResult.IsValid,
            Errors = standardResult.Errors
        });
        
        // Create a large order
        var largeOrder = new Order(
            OrderId: "ORD-002",
            AccountId: "ACC-001",
            Symbol: "MSFT",
            Side: OrderSide.Buy,
            Quantity: 1000,
            Price: 300m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var largeValidator = _factory.CreateValidator(largeOrder);
        var largeResult = largeValidator.Validate(largeOrder);
        results.Add(new
        {
            OrderType = "Large",
            OrderValue = largeOrder.Quantity * largeOrder.Price,
            ValidatorType = largeValidator.ValidatorType,
            IsValid = largeResult.IsValid,
            Errors = largeResult.Errors
        });
        
        return new PatternDemoResponse(
            Pattern: "Factory Method",
            Description: "Demonstrates factory method pattern: different validators created based on order characteristics.",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["FactoryType"] = "OrderValidatorFactory",
                ["Extensibility"] = "Easy to add new validator types without modifying existing code"
            }
        );
    }
    
    /// <summary>
    /// Test: Verify factory creates correct validators.
    /// </summary>
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Test 1: Standard order should get Standard validator
        var standardOrder = new Order(
            OrderId: "TEST-001",
            AccountId: "ACC-001",
            Symbol: "AAPL",
            Side: OrderSide.Buy,
            Quantity: 10,
            Price: 100m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var standardValidator = _factory.CreateValidator(standardOrder);
        checks.Add(new TestCheck(
            "Standard Order Validator",
            standardValidator.ValidatorType == "Standard",
            $"Created {standardValidator.ValidatorType} validator for standard order"
        ));
        
        // Test 2: Large order should get LargeOrder validator
        var largeOrder = new Order(
            OrderId: "TEST-002",
            AccountId: "ACC-001",
            Symbol: "MSFT",
            Side: OrderSide.Buy,
            Quantity: 1000,
            Price: 200m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var largeValidator = _factory.CreateValidator(largeOrder);
        checks.Add(new TestCheck(
            "Large Order Validator",
            largeValidator.ValidatorType == "LargeOrder",
            $"Created {largeValidator.ValidatorType} validator for large order (value: {largeOrder.Quantity * largeOrder.Price})"
        ));
        
        // Test 3: Validator should validate correctly
        var validationResult = standardValidator.Validate(standardOrder);
        checks.Add(new TestCheck(
            "Validation Works",
            validationResult.IsValid,
            $"Standard validator returned IsValid={validationResult.IsValid}"
        ));
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Factory Method",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
