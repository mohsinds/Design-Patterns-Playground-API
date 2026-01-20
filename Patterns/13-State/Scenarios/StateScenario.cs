using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._13_State.Contracts;
using DesignPatterns.Playground.Api.Patterns._13_State.Implementations;

namespace DesignPatterns.Playground.Api.Patterns._13_State.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing State pattern.
/// </summary>
public class StateScenario
{
    private readonly ILogger<StateScenario> _logger;
    
    public StateScenario(ILogger<StateScenario> logger)
    {
        _logger = logger;
    }
    
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Create order in Pending state
        var order = new Order(
            OrderId: "ORD-STATE-001",
            AccountId: "ACC-001",
            Symbol: "AAPL",
            Side: OrderSide.Buy,
            Quantity: 100,
            Price: 150m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var state = OrderStateFactory.CreateState(order.Status);
        results.Add(new
        {
            Step = "Initial",
            State = state.Status.ToString(),
            Order = order
        });
        
        // Transition: Pending -> Placed
        order = state.Place(order);
        state = OrderStateFactory.CreateState(order.Status);
        results.Add(new
        {
            Step = "Place",
            State = state.Status.ToString(),
            Order = order
        });
        
        // Transition: Placed -> Filled
        order = state.Fill(order, 100);
        state = OrderStateFactory.CreateState(order.Status);
        results.Add(new
        {
            Step = "Fill",
            State = state.Status.ToString(),
            Order = order
        });
        
        // Try invalid transition (should fail)
        try
        {
            order = state.Cancel(order, "Test");
            results.Add(new
            {
                Step = "Invalid Cancel",
                Success = false,
                Message = "Should have thrown exception"
            });
        }
        catch (InvalidOperationException ex)
        {
            results.Add(new
            {
                Step = "Invalid Cancel",
                Success = true,
                Message = ex.Message
            });
        }
        
        return new PatternDemoResponse(
            Pattern: "State",
            Description: "Demonstrates state pattern: encapsulates order lifecycle state transitions with validation.",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["StateTransitions"] = "Pending -> Placed -> Filled/Cancelled",
                ["TerminalStates"] = new[] { "Filled", "Cancelled", "Rejected" },
                ["Validation"] = "Invalid transitions throw exceptions"
            }
        );
    }
    
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Test 1: Valid transition Pending -> Placed
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
        
        var state = OrderStateFactory.CreateState(order.Status);
        order = state.Place(order);
        checks.Add(new TestCheck(
            "Pending to Placed Transition",
            order.Status == OrderStatus.Placed,
            $"Order status: {order.Status}"
        ));
        
        // Test 2: Valid transition Placed -> Filled
        state = OrderStateFactory.CreateState(order.Status);
        order = state.Fill(order, 10);
        checks.Add(new TestCheck(
            "Placed to Filled Transition",
            order.Status == OrderStatus.Filled,
            $"Order status: {order.Status}"
        ));
        
        // Test 3: Invalid transition (Filled -> Cancel)
        state = OrderStateFactory.CreateState(order.Status);
        try
        {
            state.Cancel(order, "Test");
            checks.Add(new TestCheck(
                "Invalid Transition Prevention",
                false,
                "Should have thrown exception"
            ));
        }
        catch (InvalidOperationException)
        {
            checks.Add(new TestCheck(
                "Invalid Transition Prevention",
                true,
                "Invalid transition correctly prevented"
            ));
        }
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "State",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
