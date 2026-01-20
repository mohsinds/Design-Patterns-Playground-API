using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._13_State.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._13_State.Implementations;

/// <summary>
/// Pending order state.
/// </summary>
public class PendingOrderState : IOrderState
{
    public OrderStatus Status => OrderStatus.Pending;
    
    public Order Place(Order order)
    {
        return order with { Status = OrderStatus.Placed, UpdatedAt = DateTime.UtcNow };
    }
    
    public Order Fill(Order order, decimal filledQuantity)
    {
        throw new InvalidOperationException("Cannot fill order in Pending state. Must place order first.");
    }
    
    public Order Cancel(Order order, string reason)
    {
        return order with { Status = OrderStatus.Cancelled, UpdatedAt = DateTime.UtcNow };
    }
    
    public Order Reject(Order order, string reason)
    {
        return order with { Status = OrderStatus.Rejected, UpdatedAt = DateTime.UtcNow };
    }
}

/// <summary>
/// Placed order state.
/// </summary>
public class PlacedOrderState : IOrderState
{
    public OrderStatus Status => OrderStatus.Placed;
    
    public Order Place(Order order)
    {
        throw new InvalidOperationException("Order is already placed.");
    }
    
    public Order Fill(Order order, decimal filledQuantity)
    {
        if (filledQuantity >= order.Quantity)
        {
            return order with { Status = OrderStatus.Filled, UpdatedAt = DateTime.UtcNow };
        }
        return order with { Status = OrderStatus.PartiallyFilled, UpdatedAt = DateTime.UtcNow };
    }
    
    public Order Cancel(Order order, string reason)
    {
        return order with { Status = OrderStatus.Cancelled, UpdatedAt = DateTime.UtcNow };
    }
    
    public Order Reject(Order order, string reason)
    {
        throw new InvalidOperationException("Cannot reject order in Placed state. Use Cancel instead.");
    }
}

/// <summary>
/// Filled order state (terminal).
/// </summary>
public class FilledOrderState : IOrderState
{
    public OrderStatus Status => OrderStatus.Filled;
    
    public Order Place(Order order)
    {
        throw new InvalidOperationException("Cannot place order in Filled state (terminal).");
    }
    
    public Order Fill(Order order, decimal filledQuantity)
    {
        throw new InvalidOperationException("Order is already filled.");
    }
    
    public Order Cancel(Order order, string reason)
    {
        throw new InvalidOperationException("Cannot cancel order in Filled state (terminal).");
    }
    
    public Order Reject(Order order, string reason)
    {
        throw new InvalidOperationException("Cannot reject order in Filled state (terminal).");
    }
}

/// <summary>
/// Cancelled order state (terminal).
/// </summary>
public class CancelledOrderState : IOrderState
{
    public OrderStatus Status => OrderStatus.Cancelled;
    
    public Order Place(Order order)
    {
        throw new InvalidOperationException("Cannot place order in Cancelled state (terminal).");
    }
    
    public Order Fill(Order order, decimal filledQuantity)
    {
        throw new InvalidOperationException("Cannot fill order in Cancelled state (terminal).");
    }
    
    public Order Cancel(Order order, string reason)
    {
        throw new InvalidOperationException("Order is already cancelled.");
    }
    
    public Order Reject(Order order, string reason)
    {
        throw new InvalidOperationException("Cannot reject order in Cancelled state (terminal).");
    }
}

/// <summary>
/// Order state factory.
/// </summary>
public static class OrderStateFactory
{
    public static IOrderState CreateState(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => new PendingOrderState(),
            OrderStatus.Placed => new PlacedOrderState(),
            OrderStatus.Filled => new FilledOrderState(),
            OrderStatus.Cancelled => new CancelledOrderState(),
            _ => new PendingOrderState()
        };
    }
}
