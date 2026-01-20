using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._12_Mediator.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Mediator pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Defines an object that encapsulates how a set of objects interact, promoting 
/// loose coupling by keeping objects from referring to each other explicitly.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Many objects communicate with each other (many-to-many dependencies)
/// • Want to reduce coupling between components
/// • Need centralized communication hub
/// • Want to simplify object interactions
/// • Need to add/remove components without affecting others
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use for simple one-to-one communication (use direct calls)
/// • DO NOT use when mediator becomes a god object
/// • Avoid when you only have 2-3 components
/// • Don't use if mediator becomes too complex
/// • Anti-pattern: Mediator that knows too much about all components
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Request Routing: Mediator routes requests to appropriate handlers (CQRS)
/// 2. Microservices Communication: API gateway mediates between services
/// 3. Event Coordination: Mediator coordinates events between components
/// 4. Workflow Orchestration: Mediator coordinates workflow steps
/// 5. Message Routing: Mediator routes messages to appropriate processors
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to reduce many-to-many dependencies?" → Mediator
/// Q: "Design a request routing system" → Mediator
/// Q: "How to decouple components that need to communicate?" → Mediator
/// Q: "Implement CQRS command/query routing" → Mediator
/// Q: "Coordinate interactions between multiple services" → Mediator
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Mediators are stateless - safe for multi-instance deployment
/// • Each pod can have its own mediator instance
/// • Mediator routes requests (stateless routing)
/// • No shared state between mediator instances
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Log mediator routing decisions, handler selections
/// • Metrics: Track mediator usage, routing success/failure rates
/// • Tracing: Trace spans for mediator routing and handler execution
/// • Audit: Log routing decisions for compliance
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - mock mediator, test component interactions
/// • Test mediator routing logic independently
/// • Mock handlers for testing mediator
/// • Test mediator decouples components correctly
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • Request Routing: Routing requests to appropriate handlers
/// • Component Coordination: Coordinating interactions between components
/// • Dependency Reduction: Reducing many-to-many dependencies
/// • Centralized Communication: Centralizing communication logic
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 7/10
/// ========================================================================
/// Common in interviews, especially for CQRS and microservices architecture.
/// Important for understanding how to reduce coupling and centralize communication.
/// Frequently used in modern application frameworks (MediatR).
/// </summary>
[ApiController]
[Route("api/patterns/mediator")]
public class MediatorController : ControllerBase
{
    private readonly MediatorScenario _scenario;
    
    public MediatorController(MediatorScenario scenario)
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
