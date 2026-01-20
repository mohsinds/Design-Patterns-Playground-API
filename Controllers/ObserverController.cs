using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._09_Observer.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Observer pattern (Pub-Sub).
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Defines a one-to-many dependency between objects so that when one object 
/// changes state, all its dependents are notified and updated automatically.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Need to notify multiple objects about state changes
/// • Want to decouple subject from observers
/// • Need event-driven architecture
/// • Want loose coupling between components
/// • Need to broadcast events to multiple subscribers
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use for one-to-one notifications (use callbacks)
/// • DO NOT use when order of notifications matters critically
/// • Avoid when observers need to know about each other
/// • Don't use if you need guaranteed delivery (use message queues)
/// • Anti-pattern: Observer that modifies subject state (circular dependency)
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Domain Events: Order placed, filled, cancelled events with multiple handlers
/// 2. Market Data Distribution: Price updates broadcast to multiple subscribers
/// 3. Audit Logging: Business events trigger audit log entries
/// 4. Notification Systems: User actions trigger email, SMS, push notifications
/// 5. Event Sourcing: Domain events published to event store and handlers
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to notify multiple components about state changes?" → Observer
/// Q: "Design an event-driven system" → Observer
/// Q: "Implement pub-sub messaging" → Observer
/// Q: "How to decouple event publishers from subscribers?" → Observer
/// Q: "Design domain events system" → Observer
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Use distributed message broker (Kafka, RabbitMQ) for cross-pod communication
/// • Each pod can publish/subscribe to events
/// • Event bus should be external (not in-memory)
/// • Use outbox pattern for reliable event publishing
/// • Event handlers should be idempotent (handle duplicate events)
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Log all events published, handlers invoked, failures
/// • Metrics: Track event publishing rates, handler execution times
/// • Tracing: Trace spans for event flow (publisher → broker → handlers)
/// • Audit: All events logged for compliance (domain events audit trail)
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - subscribe test handlers, publish events, verify
/// • Mock event bus for testing publishers
/// • Test handler logic independently
/// • Test event ordering and delivery
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • Event-Driven Architecture: Loose coupling via events
/// • Domain Events: Business events with multiple handlers
/// • Pub-Sub Messaging: Publish events, multiple subscribers
/// • State Change Notifications: Notify observers of state changes
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 9/10
/// ========================================================================
/// Extremely common in interviews, especially for event-driven architecture.
/// Critical for understanding pub-sub, domain events, and microservices communication.
/// Frequently asked in system design interviews.
/// </summary>
[ApiController]
[Route("api/patterns/observer")]
public class ObserverController : ControllerBase
{
    private readonly ObserverScenario _scenario;
    
    public ObserverController(ObserverScenario scenario)
    {
        _scenario = scenario;
    }
    
    [HttpGet("demo")]
    [ProducesResponseType(typeof(PatternDemoResponse), 200)]
    public IActionResult Demo() => Ok(_scenario.RunDemo());
    
    [HttpGet("test")]
    [ProducesResponseType(typeof(PatternTestResponse), 200)]
    public IActionResult Test() => Ok(_scenario.RunTest());
}
