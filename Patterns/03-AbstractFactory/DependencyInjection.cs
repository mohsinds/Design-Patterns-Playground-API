using DesignPatterns.Playground.Api.Infrastructure;
using DesignPatterns.Playground.Api.Patterns._03_AbstractFactory.Contracts;
using DesignPatterns.Playground.Api.Patterns._03_AbstractFactory.Implementations;
using DesignPatterns.Playground.Api.Patterns._03_AbstractFactory.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._03_AbstractFactory;

public static class DependencyInjection
{
    public static IServiceCollection AddAbstractFactoryPattern(this IServiceCollection services)
    {
        // Register factories - in real system, you'd use a factory selector based on config
        services.AddScoped<IPaymentGatewayFactory, StripeGatewayFactory>(sp =>
            new StripeGatewayFactory(sp.GetRequiredService<ILogger<FakeStripeGateway>>()));
        services.AddScoped<IPaymentGatewayFactory, PayPalGatewayFactory>(sp =>
            new PayPalGatewayFactory(sp.GetRequiredService<ILogger<FakePayPalGateway>>()));
        
        // For demo, we'll register named factories
        services.AddScoped<StripeGatewayFactory>();
        services.AddScoped<PayPalGatewayFactory>();
        services.AddScoped<AbstractFactoryScenario>(sp => new AbstractFactoryScenario(
            sp.GetRequiredService<StripeGatewayFactory>(),
            sp.GetRequiredService<PayPalGatewayFactory>(),
            sp.GetRequiredService<ILogger<AbstractFactoryScenario>>()
        ));
        
        return services;
    }
}
