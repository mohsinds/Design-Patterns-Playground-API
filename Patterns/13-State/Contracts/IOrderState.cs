using DesignPatterns.Playground.Api.Domain;

namespace DesignPatterns.Playground.Api.Patterns._13_State.Contracts;

/// <summary>
/// Order state interface.
/// State pattern: encapsulates order lifecycle state transitions.
/// </summary>
public interface IOrderState
{
    /// <summary>
    /// Get current status.
    /// </summary>
    OrderStatus Status { get; }
    
    /// <summary>
    /// Place the order (transition to Placed).
    /// </summary>
    Order Place(Order order);
    
    /// <summary>
    /// Fill the order (transition to Filled).
    /// </summary>
    Order Fill(Order order, decimal filledQuantity);
    
    /// <summary>
    /// Cancel the order (transition to Cancelled).
    /// </summary>
    Order Cancel(Order order, string reason);
    
    /// <summary>
    /// Reject the order (transition to Rejected).
    /// </summary>
    Order Reject(Order order, string reason);
}
