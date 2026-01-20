using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._09_Observer.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._09_Observer.Implementations;

/// <summary>
/// Order placed domain event.
/// </summary>
public record OrderPlacedEvent(
    string OrderId,
    string AccountId,
    string Symbol,
    decimal Quantity,
    decimal Price,
    DateTime Timestamp
) : IDomainEvent
{
    public string EventId { get; } = $"EVT-{Guid.NewGuid():N}";
    public string EventType => "OrderPlaced";
}

/// <summary>
/// Order filled domain event.
/// </summary>
public record OrderFilledEvent(
    string OrderId,
    string AccountId,
    decimal FilledQuantity,
    decimal FillPrice,
    DateTime Timestamp
) : IDomainEvent
{
    public string EventId { get; } = $"EVT-{Guid.NewGuid():N}";
    public string EventType => "OrderFilled";
}

/// <summary>
/// Order cancelled domain event.
/// </summary>
public record OrderCancelledEvent(
    string OrderId,
    string AccountId,
    string Reason,
    DateTime Timestamp
) : IDomainEvent
{
    public string EventId { get; } = $"EVT-{Guid.NewGuid():N}";
    public string EventType => "OrderCancelled";
}
