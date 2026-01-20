using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Infrastructure;
using DesignPatterns.Playground.Api.Patterns._03_AbstractFactory.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._03_AbstractFactory.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Abstract Factory pattern.
/// </summary>
public class AbstractFactoryScenario
{
    private readonly IPaymentGatewayFactory _stripeFactory;
    private readonly IPaymentGatewayFactory _payPalFactory;
    private readonly ILogger<AbstractFactoryScenario> _logger;
    
    public AbstractFactoryScenario(
        IPaymentGatewayFactory stripeFactory,
        IPaymentGatewayFactory payPalFactory,
        ILogger<AbstractFactoryScenario> logger)
    {
        _stripeFactory = stripeFactory;
        _payPalFactory = payPalFactory;
        _logger = logger;
    }
    
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Create Stripe family
        var stripeGateway = _stripeFactory.CreatePaymentGateway();
        var stripeConfig = _stripeFactory.CreateConfiguration();
        results.Add(new
        {
            Factory = "Stripe",
            Gateway = stripeGateway.ProviderName,
            Config = stripeConfig
        });
        
        // Create PayPal family
        var payPalGateway = _payPalFactory.CreatePaymentGateway();
        var payPalConfig = _payPalFactory.CreateConfiguration();
        results.Add(new
        {
            Factory = "PayPal",
            Gateway = payPalGateway.ProviderName,
            Config = payPalConfig
        });
        
        // Process payment with Stripe
        var stripePayment = new PaymentRequest(
            TransactionId: "TXN-STRIPE-001",
            Amount: 100.50m,
            Currency: "USD",
            AccountId: "ACC-001"
        );
        var stripeResult = stripeGateway.ProcessPaymentAsync(stripePayment).Result;
        
        results.Add(new
        {
            Payment = "Stripe Payment",
            Result = stripeResult
        });
        
        return new PatternDemoResponse(
            Pattern: "Abstract Factory",
            Description: "Demonstrates abstract factory pattern: creates families of related objects (gateway + config).",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["FactoryCount"] = 2,
                ["Extensibility"] = "Easy to add new gateway families without modifying existing code"
            }
        );
    }
    
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Test 1: Stripe factory creates Stripe gateway
        var stripeGateway = _stripeFactory.CreatePaymentGateway();
        checks.Add(new TestCheck(
            "Stripe Factory Creates Stripe Gateway",
            stripeGateway.ProviderName == "Stripe",
            $"Created {stripeGateway.ProviderName} gateway"
        ));
        
        // Test 2: PayPal factory creates PayPal gateway
        var payPalGateway = _payPalFactory.CreatePaymentGateway();
        checks.Add(new TestCheck(
            "PayPal Factory Creates PayPal Gateway",
            payPalGateway.ProviderName == "PayPal",
            $"Created {payPalGateway.ProviderName} gateway"
        ));
        
        // Test 3: Config matches gateway
        var stripeConfig = _stripeFactory.CreateConfiguration();
        checks.Add(new TestCheck(
            "Stripe Config Matches Gateway",
            stripeConfig.ProviderName == stripeGateway.ProviderName,
            $"Config provider {stripeConfig.ProviderName} matches gateway {stripeGateway.ProviderName}"
        ));
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Abstract Factory",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
