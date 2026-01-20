using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Infrastructure;
using DesignPatterns.Playground.Api.Patterns._07_Decorator.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._07_Decorator.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Decorator pattern.
/// </summary>
public class DecoratorScenario
{
    private readonly IPaymentService _paymentService;
    private readonly IMetrics _metrics;
    private readonly ILogger<DecoratorScenario> _logger;
    
    public DecoratorScenario(
        IPaymentService paymentService,
        IMetrics metrics,
        ILogger<DecoratorScenario> logger)
    {
        _paymentService = paymentService;
        _metrics = metrics;
        _logger = logger;
    }
    
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Process payment with decorators (logging, metrics, retry)
        var paymentRequest = new PaymentRequest(
            TransactionId: "TXN-DEC-001",
            Amount: 250.75m,
            Currency: "USD",
            AccountId: "ACC-001"
        );
        
        var paymentResult = _paymentService.ProcessPaymentAsync(paymentRequest).Result;
        
        results.Add(new
        {
            PaymentRequest = paymentRequest,
            PaymentResult = paymentResult,
            Decorators = new[] { "Logging", "Metrics", "Retry" }
        });
        
        // Get metrics snapshot
        if (_metrics is InMemoryMetrics inMemoryMetrics)
        {
            results.Add(new
            {
                Metrics = inMemoryMetrics.GetSnapshot()
            });
        }
        
        return new PatternDemoResponse(
            Pattern: "Decorator",
            Description: "Demonstrates decorator pattern: adds cross-cutting concerns (logging, metrics, retries) dynamically without modifying core service.",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["DecoratorStack"] = "Retry -> Metrics -> Logging -> Core",
                ["SeparationOfConcerns"] = true
            }
        );
    }
    
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Test 1: Payment service processes payment
        var paymentRequest = new PaymentRequest(
            TransactionId: "TXN-TEST-001",
            Amount: 100m,
            Currency: "USD",
            AccountId: "ACC-TEST"
        );
        
        var result = _paymentService.ProcessPaymentAsync(paymentRequest).Result;
        checks.Add(new TestCheck(
            "Payment Processing",
            result != null,
            $"Payment processed: Success={result.Success}"
        ));
        
        // Test 2: Metrics are recorded
        if (_metrics is InMemoryMetrics inMemoryMetrics)
        {
            var snapshot = inMemoryMetrics.GetSnapshot();
            var hasMetrics = snapshot.ContainsKey("counters") || snapshot.ContainsKey("durations");
            checks.Add(new TestCheck(
                "Metrics Recording",
                hasMetrics,
                "Metrics were recorded by decorator"
            ));
        }
        
        // Test 3: Decorator chain works
        checks.Add(new TestCheck(
            "Decorator Chain",
            result.TransactionId == paymentRequest.TransactionId,
            "Decorator chain preserved request/response flow"
        ));
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Decorator",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
