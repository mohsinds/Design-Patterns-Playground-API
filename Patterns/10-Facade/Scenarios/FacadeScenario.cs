using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._10_Facade.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._10_Facade.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Facade pattern.
/// </summary>
public class FacadeScenario
{
    private readonly ITradingFacade _facade;
    private readonly ILogger<FacadeScenario> _logger;
    
    public FacadeScenario(
        ITradingFacade facade,
        ILogger<FacadeScenario> logger)
    {
        _facade = facade;
        _logger = logger;
    }
    
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Place order via facade (simplifies complex subsystem)
        var placeRequest = new PlaceOrderRequest(
            AccountId: "ACC-001",
            Symbol: "AAPL",
            Side: OrderSide.Buy,
            Quantity: 100,
            LimitPrice: 150m
        );
        
        var placeResult = _facade.PlaceOrderAsync(placeRequest).Result;
        results.Add(new
        {
            Action = "Place Order",
            Request = placeRequest,
            Result = placeResult
        });
        
        // Cancel order via facade
        if (placeResult.Success && placeResult.Order != null)
        {
            var cancelRequest = new CancelOrderRequest(
                OrderId: placeResult.Order.OrderId,
                AccountId: "ACC-001"
            );
            
            var cancelResult = _facade.CancelOrderAsync(cancelRequest).Result;
            results.Add(new
            {
                Action = "Cancel Order",
                Request = cancelRequest,
                Result = cancelResult
            });
        }
        
        return new PatternDemoResponse(
            Pattern: "Facade",
            Description: "Demonstrates facade pattern: simplifies complex trading subsystem (validation, risk, repository, events) behind simple interface.",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["SubsystemComponents"] = new[] { "Validator", "Repository", "CommandHandler", "EventBus" },
                ["SimplifiedInterface"] = true
            }
        );
    }
    
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Test 1: Facade places order successfully
        var placeRequest = new PlaceOrderRequest(
            AccountId: "ACC-TEST",
            Symbol: "TEST",
            Side: OrderSide.Buy,
            Quantity: 10,
            LimitPrice: 100m
        );
        
        var placeResult = _facade.PlaceOrderAsync(placeRequest).Result;
        checks.Add(new TestCheck(
            "Facade Places Order",
            placeResult.Success && placeResult.Order != null,
            $"Order placed: {placeResult.Order?.OrderId}"
        ));
        
        // Test 2: Facade validates orders
        var invalidRequest = new PlaceOrderRequest(
            AccountId: "ACC-TEST",
            Symbol: "",
            Side: OrderSide.Buy,
            Quantity: -10,
            LimitPrice: 100m
        );
        
        var invalidResult = _facade.PlaceOrderAsync(invalidRequest).Result;
        checks.Add(new TestCheck(
            "Facade Validates Orders",
            !invalidResult.Success && invalidResult.Errors != null && invalidResult.Errors.Count > 0,
            $"Validation errors: {string.Join(", ", invalidResult.Errors ?? new List<string>())}"
        ));
        
        // Test 3: Facade cancels orders
        if (placeResult.Success && placeResult.Order != null)
        {
            var cancelRequest = new CancelOrderRequest(
                OrderId: placeResult.Order.OrderId,
                AccountId: "ACC-TEST"
            );
            
            var cancelResult = _facade.CancelOrderAsync(cancelRequest).Result;
            checks.Add(new TestCheck(
                "Facade Cancels Orders",
                cancelResult.Success,
                $"Order cancelled: {cancelResult.Success}"
            ));
        }
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Facade",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
