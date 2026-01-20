using DesignPatterns.Playground.Api.Infrastructure;
using DesignPatterns.Playground.Api.Patterns._03_AbstractFactory.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._03_AbstractFactory.Implementations;

/// <summary>
/// Stripe payment gateway factory.
/// Creates Stripe gateway and its configuration as a family.
/// </summary>
public class StripeGatewayFactory : IPaymentGatewayFactory
{
    private readonly ILogger<FakeStripeGateway> _logger;
    
    public StripeGatewayFactory(ILogger<FakeStripeGateway> logger)
    {
        _logger = logger;
    }
    
    public string FactoryType => "Stripe";
    
    public IPaymentGateway CreatePaymentGateway()
    {
        return new FakeStripeGateway(_logger);
    }
    
    public GatewayConfig CreateConfiguration()
    {
        return new GatewayConfig(
            ProviderName: "Stripe",
            Settings: new Dictionary<string, string>
            {
                ["ApiKey"] = "sk_test_...",
                ["WebhookSecret"] = "whsec_...",
                ["Timeout"] = "30s",
                ["RetryPolicy"] = "exponential-backoff"
            }
        );
    }
}
