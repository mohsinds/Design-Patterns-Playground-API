namespace DesignPatterns.Playground.Api.Patterns._09_Observer.Contracts;

/// <summary>
/// Domain event interface.
/// Observer pattern: enables pub-sub for domain events.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Event ID for tracking.
    /// </summary>
    string EventId { get; }
    
    /// <summary>
    /// Event timestamp.
    /// </summary>
    DateTime Timestamp { get; }
    
    /// <summary>
    /// Event type name.
    /// </summary>
    string EventType { get; }
}
