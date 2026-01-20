using DesignPatterns.Playground.Api.Patterns._09_Observer.Contracts;
using DesignPatterns.Playground.Api.Patterns._09_Observer.Implementations;

namespace DesignPatterns.Playground.Api.Patterns._09_Observer.Implementations;

/// <summary>
/// Handler for order placed events (e.g., sends notification).
/// </summary>
public class OrderPlacedEventHandler : IEventHandler<OrderPlacedEvent>
{
    private readonly ILogger<OrderPlacedEventHandler> _logger;
    
    public OrderPlacedEventHandler(ILogger<OrderPlacedEventHandler> logger)
    {
        _logger = logger;
    }
    
    public Task HandleAsync(OrderPlacedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Order placed event handled: OrderId={OrderId}, Symbol={Symbol}, Quantity={Quantity}",
            @event.OrderId, @event.Symbol, @event.Quantity);
        
        // In production: send notification, update cache, etc.
        return Task.CompletedTask;
    }
}

/// <summary>
/// Handler for order filled events (e.g., updates ledger).
/// </summary>
public class OrderFilledEventHandler : IEventHandler<OrderFilledEvent>
{
    private readonly ILogger<OrderFilledEventHandler> _logger;
    
    public OrderFilledEventHandler(ILogger<OrderFilledEventHandler> logger)
    {
        _logger = logger;
    }
    
    public Task HandleAsync(OrderFilledEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Order filled event handled: OrderId={OrderId}, FilledQuantity={FilledQuantity}, FillPrice={FillPrice}",
            @event.OrderId, @event.FilledQuantity, @event.FillPrice);
        
        // In production: update ledger, calculate P&L, etc.
        return Task.CompletedTask;
    }
}

/// <summary>
/// Handler for order cancelled events (e.g., releases reserved funds).
/// </summary>
public class OrderCancelledEventHandler : IEventHandler<OrderCancelledEvent>
{
    private readonly ILogger<OrderCancelledEventHandler> _logger;
    
    public OrderCancelledEventHandler(ILogger<OrderCancelledEventHandler> logger)
    {
        _logger = logger;
    }
    
    public Task HandleAsync(OrderCancelledEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Order cancelled event handled: OrderId={OrderId}, Reason={Reason}",
            @event.OrderId, @event.Reason);
        
        // In production: release reserved funds, update risk limits, etc.
        return Task.CompletedTask;
    }
}
