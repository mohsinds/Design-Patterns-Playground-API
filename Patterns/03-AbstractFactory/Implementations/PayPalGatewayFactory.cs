using DesignPatterns.Playground.Api.Infrastructure;
using DesignPatterns.Playground.Api.Patterns._03_AbstractFactory.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._03_AbstractFactory.Implementations;

/// <summary>
/// PayPal payment gateway factory.
/// Creates PayPal gateway and its configuration as a family.
/// </summary>
public class PayPalGatewayFactory : IPaymentGatewayFactory
{
    private readonly ILogger<FakePayPalGateway> _logger;
    
    public PayPalGatewayFactory(ILogger<FakePayPalGateway> logger)
    {
        _logger = logger;
    }
    
    public string FactoryType => "PayPal";
    
    public IPaymentGateway CreatePaymentGateway()
    {
        return new FakePayPalGateway(_logger);
    }
    
    public GatewayConfig CreateConfiguration()
    {
        return new GatewayConfig(
            ProviderName: "PayPal",
            Settings: new Dictionary<string, string>
            {
                ["ClientId"] = "paypal_client_...",
                ["ClientSecret"] = "paypal_secret_...",
                ["Mode"] = "sandbox",
                ["Timeout"] = "45s",
                ["RetryPolicy"] = "linear"
            }
        );
    }
}
