namespace DesignPatterns.Playground.Api.Patterns._09_Observer.Contracts;

/// <summary>
/// Event bus interface for pub-sub.
/// Observer pattern: decouples event publishers from subscribers.
/// In production, this would integrate with Kafka, RabbitMQ, etc.
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publish an event.
    /// </summary>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IDomainEvent;
    
    /// <summary>
    /// Subscribe to events of type TEvent.
    /// </summary>
    void Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IDomainEvent;
}
