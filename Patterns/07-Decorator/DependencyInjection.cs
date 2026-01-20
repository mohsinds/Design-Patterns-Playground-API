using DesignPatterns.Playground.Api.Infrastructure;
using DesignPatterns.Playground.Api.Patterns._07_Decorator.Contracts;
using DesignPatterns.Playground.Api.Patterns._07_Decorator.Implementations;
using DesignPatterns.Playground.Api.Patterns._07_Decorator.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._07_Decorator;

public static class DependencyInjection
{
    public static IServiceCollection AddDecoratorPattern(this IServiceCollection services)
    {
        // Register core service
        services.AddScoped<IPaymentService>(sp =>
        {
            var gateway = sp.GetRequiredService<IPaymentGateway>();
            var logger = sp.GetRequiredService<ILogger<PaymentService>>();
            var core = new PaymentService(gateway, logger);
            
            // Apply decorators in reverse order (last decorator wraps first)
            var metrics = sp.GetRequiredService<IMetrics>();
            var retryLogger = sp.GetRequiredService<ILogger<RetryPaymentServiceDecorator>>();
            var loggingLogger = sp.GetRequiredService<ILogger<LoggingPaymentServiceDecorator>>();
            
            // Decorator chain: Retry -> Metrics -> Logging -> Core
            var withLogging = new LoggingPaymentServiceDecorator(core, loggingLogger);
            var withMetrics = new MetricsPaymentServiceDecorator(withLogging, metrics);
            var withRetry = new RetryPaymentServiceDecorator(withMetrics, retryLogger);
            
            return withRetry;
        });
        
        services.AddScoped<DecoratorScenario>();
        
        return services;
    }
}
