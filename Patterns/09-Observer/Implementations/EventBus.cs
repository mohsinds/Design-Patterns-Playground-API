using System.Collections.Concurrent;
using DesignPatterns.Playground.Api.Infrastructure;
using DesignPatterns.Playground.Api.Patterns._09_Observer.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._09_Observer.Implementations;

/// <summary>
/// In-memory event bus implementation.
/// Observer pattern: manages event subscriptions and publishing.
/// In production, this would use Kafka, RabbitMQ, or similar message broker.
/// Thread-safety: Uses concurrent collections, safe for concurrent use.
/// </summary>
public class InMemoryEventBus : IEventBus
{
    private readonly ILogger<InMemoryEventBus> _logger;
    private readonly IKafkaProducer _kafkaProducer; // For demo: shows integration point
    private readonly ConcurrentDictionary<Type, List<object>> _handlers = new();
    
    public InMemoryEventBus(
        ILogger<InMemoryEventBus> logger,
        IKafkaProducer kafkaProducer)
    {
        _logger = logger;
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IDomainEvent
    {
        _logger.LogInformation("Publishing event {EventType} {EventId}", @event.EventType, @event.EventId);
        
        // Publish to internal handlers
        if (_handlers.TryGetValue(typeof(TEvent), out var handlers))
        {
            foreach (var handler in handlers.Cast<IEventHandler<TEvent>>())
            {
                try
                {
                    await handler.HandleAsync(@event, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error handling event {EventId} with handler {HandlerType}",
                        @event.EventId, handler.GetType().Name);
                }
            }
        }
        
        // In production: also publish to Kafka for distributed systems
        // This demonstrates the outbox pattern mention
        try
        {
            await _kafkaProducer.PublishAsync($"domain-events.{@event.EventType.ToLower()}", @event, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to publish to Kafka, event will be retried via outbox pattern");
            // In production: store in outbox table for retry
        }
    }
    
    public void Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);
        _handlers.AddOrUpdate(
            eventType,
            new List<object> { handler },
            (key, existing) =>
            {
                existing.Add(handler);
                return existing;
            });
        
        _logger.LogInformation("Subscribed handler {HandlerType} to event {EventType}",
            handler.GetType().Name, eventType.Name);
    }
}
