using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._12_Mediator.Contracts;
using DesignPatterns.Playground.Api.Patterns._12_Mediator.Implementations;

namespace DesignPatterns.Playground.Api.Patterns._12_Mediator.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Mediator pattern.
/// </summary>
public class MediatorScenario
{
    private readonly IMediator _mediator;
    private readonly ILogger<MediatorScenario> _logger;
    
    public MediatorScenario(
        IMediator mediator,
        ILogger<MediatorScenario> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Create order via mediator
        var createRequest = new CreateOrderRequest(
            AccountId: "ACC-001",
            Symbol: "AAPL",
            Side: OrderSide.Buy,
            Quantity: 100,
            Price: 150m
        );
        
        var order = _mediator.SendAsync<Order>(createRequest).Result;
        results.Add(new
        {
            Action = "Create Order via Mediator",
            Request = createRequest,
            Order = order
        });
        
        // Get order via mediator
        var getRequest = new GetOrderRequest(order.OrderId);
        var retrieved = _mediator.SendAsync<Order?>(getRequest).Result;
        
        results.Add(new
        {
            Action = "Get Order via Mediator",
            Request = getRequest,
            Order = retrieved
        });
        
        return new PatternDemoResponse(
            Pattern: "Mediator",
            Description: "Demonstrates mediator pattern: routes requests to handlers, reducing many-to-many dependencies between components.",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["Decoupling"] = "Components don't know about each other, only the mediator",
                ["RequestRouting"] = "Mediator routes requests to appropriate handlers"
            }
        );
    }
    
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Test 1: Mediator routes create request
        var createRequest = new CreateOrderRequest(
            AccountId: "ACC-TEST",
            Symbol: "TEST",
            Side: OrderSide.Buy,
            Quantity: 10,
            Price: 100m
        );
        
        var order = _mediator.SendAsync<Order>(createRequest).Result;
        checks.Add(new TestCheck(
            "Mediator Routes Create Request",
            order != null && order.OrderId != null,
            $"Created order {order.OrderId} via mediator"
        ));
        
        // Test 2: Mediator routes get request
        var getRequest = new GetOrderRequest(order.OrderId);
        var retrieved = _mediator.SendAsync<Order?>(getRequest).Result;
        
        checks.Add(new TestCheck(
            "Mediator Routes Get Request",
            retrieved != null && retrieved.OrderId == order.OrderId,
            $"Retrieved order {retrieved?.OrderId} via mediator"
        ));
        
        // Test 3: Mediator handles missing order
        var missingRequest = new GetOrderRequest("NONEXISTENT");
        var missing = _mediator.SendAsync<Order?>(missingRequest).Result;
        
        checks.Add(new TestCheck(
            "Mediator Handles Missing Order",
            missing == null,
            "Mediator correctly returns null for missing order"
        ));
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Mediator",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
