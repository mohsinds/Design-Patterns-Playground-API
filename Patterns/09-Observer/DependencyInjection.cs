using DesignPatterns.Playground.Api.Patterns._09_Observer.Contracts;
using DesignPatterns.Playground.Api.Patterns._09_Observer.Implementations;
using DesignPatterns.Playground.Api.Patterns._09_Observer.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._09_Observer;

public static class DependencyInjection
{
    public static IServiceCollection AddObserverPattern(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, InMemoryEventBus>();
        
        // Register event handlers
        services.AddScoped<IEventHandler<OrderPlacedEvent>, OrderPlacedEventHandler>();
        services.AddScoped<IEventHandler<OrderFilledEvent>, OrderFilledEventHandler>();
        services.AddScoped<IEventHandler<OrderCancelledEvent>, OrderCancelledEventHandler>();
        
        // Subscribe handlers to event bus (done on startup)
        services.AddHostedService<EventBusSubscriberService>();
        
        services.AddScoped<ObserverScenario>();
        
        return services;
    }
}

/// <summary>
/// Service to subscribe event handlers on startup.
/// </summary>
public class EventBusSubscriberService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    
    public EventBusSubscriberService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
        
        // Subscribe handlers
        var orderPlacedHandler = scope.ServiceProvider.GetRequiredService<IEventHandler<OrderPlacedEvent>>();
        var orderFilledHandler = scope.ServiceProvider.GetRequiredService<IEventHandler<OrderFilledEvent>>();
        var orderCancelledHandler = scope.ServiceProvider.GetRequiredService<IEventHandler<OrderCancelledEvent>>();
        
        eventBus.Subscribe(orderPlacedHandler);
        eventBus.Subscribe(orderFilledHandler);
        eventBus.Subscribe(orderCancelledHandler);
        
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
