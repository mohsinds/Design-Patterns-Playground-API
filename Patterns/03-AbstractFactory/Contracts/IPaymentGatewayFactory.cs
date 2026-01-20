using DesignPatterns.Playground.Api.Infrastructure;

namespace DesignPatterns.Playground.Api.Patterns._03_AbstractFactory.Contracts;

/// <summary>
/// Abstract factory for creating payment gateway families.
/// Abstract Factory pattern: creates families of related objects (payment gateways + their configurations).
/// </summary>
public interface IPaymentGatewayFactory
{
    /// <summary>
    /// Create a payment gateway.
    /// </summary>
    IPaymentGateway CreatePaymentGateway();
    
    /// <summary>
    /// Create gateway-specific configuration.
    /// </summary>
    GatewayConfig CreateConfiguration();
    
    /// <summary>
    /// Get factory type name.
    /// </summary>
    string FactoryType { get; }
}

/// <summary>
/// Gateway configuration.
/// </summary>
public record GatewayConfig(string ProviderName, Dictionary<string, string> Settings);
