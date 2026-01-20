using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._09_Observer.Contracts;
using DesignPatterns.Playground.Api.Patterns._09_Observer.Implementations;

namespace DesignPatterns.Playground.Api.Patterns._09_Observer.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Observer pattern.
/// </summary>
public class ObserverScenario
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<ObserverScenario> _logger;
    
    public ObserverScenario(
        IEventBus eventBus,
        ILogger<ObserverScenario> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }
    
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Publish order placed event
        var orderPlacedEvent = new OrderPlacedEvent(
            OrderId: "ORD-OBS-001",
            AccountId: "ACC-001",
            Symbol: "AAPL",
            Quantity: 100,
            Price: 150m,
            Timestamp: DateTime.UtcNow
        );
        
        _eventBus.PublishAsync(orderPlacedEvent).Wait();
        results.Add(new
        {
            Event = "OrderPlaced",
            EventId = orderPlacedEvent.EventId,
            OrderId = orderPlacedEvent.OrderId
        });
        
        // Publish order filled event
        var orderFilledEvent = new OrderFilledEvent(
            OrderId: "ORD-OBS-001",
            AccountId: "ACC-001",
            FilledQuantity: 100,
            FillPrice: 150.25m,
            Timestamp: DateTime.UtcNow
        );
        
        _eventBus.PublishAsync(orderFilledEvent).Wait();
        results.Add(new
        {
            Event = "OrderFilled",
            EventId = orderFilledEvent.EventId,
            OrderId = orderFilledEvent.OrderId
        });
        
        return new PatternDemoResponse(
            Pattern: "Observer / Pub-Sub",
            Description: "Demonstrates observer pattern: domain events published and handled by multiple subscribers. In production, integrates with Kafka for distributed pub-sub.",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["EventTypes"] = new[] { "OrderPlaced", "OrderFilled", "OrderCancelled" },
                ["KafkaIntegration"] = "Events also published to Kafka (simulated)",
                ["OutboxPattern"] = "Failed Kafka publishes would use outbox pattern for retry"
            }
        );
    }
    
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Test 1: Event can be published
        var orderPlacedEvent = new OrderPlacedEvent(
            OrderId: "ORD-TEST",
            AccountId: "ACC-TEST",
            Symbol: "TEST",
            Quantity: 10,
            Price: 100m,
            Timestamp: DateTime.UtcNow
        );
        
        try
        {
            _eventBus.PublishAsync(orderPlacedEvent).Wait();
            checks.Add(new TestCheck(
                "Event Publishing",
                true,
                $"Published event {orderPlacedEvent.EventId}"
            ));
        }
        catch (Exception ex)
        {
            checks.Add(new TestCheck(
                "Event Publishing",
                false,
                $"Failed to publish event: {ex.Message}"
            ));
        }
        
        // Test 2: Event has required properties
        checks.Add(new TestCheck(
            "Event Properties",
            !string.IsNullOrEmpty(orderPlacedEvent.EventId) && !string.IsNullOrEmpty(orderPlacedEvent.EventType),
            $"EventId={orderPlacedEvent.EventId}, EventType={orderPlacedEvent.EventType}"
        ));
        
        // Test 3: Multiple event types work
        var orderCancelledEvent = new OrderCancelledEvent(
            OrderId: "ORD-TEST",
            AccountId: "ACC-TEST",
            Reason: "User request",
            Timestamp: DateTime.UtcNow
        );
        
        try
        {
            _eventBus.PublishAsync(orderCancelledEvent).Wait();
            checks.Add(new TestCheck(
                "Multiple Event Types",
                true,
                $"Published {orderCancelledEvent.EventType} event"
            ));
        }
        catch (Exception ex)
        {
            checks.Add(new TestCheck(
                "Multiple Event Types",
                false,
                $"Failed: {ex.Message}"
            ));
        }
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Observer / Pub-Sub",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
