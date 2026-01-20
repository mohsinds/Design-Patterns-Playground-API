using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._07_Decorator.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Decorator pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Attaches additional responsibilities to objects dynamically without 
/// modifying their structure. Provides flexible alternative to subclassing.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Need to add behavior to objects at runtime
/// • Want to add cross-cutting concerns (logging, caching, retries)
/// • Need to combine behaviors flexibly
/// • Want to avoid class explosion from inheritance
/// • Need to add features without modifying existing code
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use when behavior should be part of the core class
/// • DO NOT use for simple method wrapping (use AOP frameworks)
/// • Avoid when decorators add too much overhead
/// • Don't use if you need to remove behavior (decorators are additive)
/// • Anti-pattern: Decorator that changes core behavior instead of adding
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Payment Service Decorators: Add logging, metrics, retries, caching layers
/// 2. API Client Decorators: Add authentication, rate limiting, circuit breakers
/// 3. Repository Decorators: Add caching, audit logging, performance monitoring
/// 4. Message Handler Decorators: Add retry logic, dead letter queue, tracing
/// 5. Service Decorators: Add transaction management, security checks, validation
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to add logging to a service without modifying it?" → Decorator
/// Q: "Add retry logic to payment processing" → Decorator
/// Q: "Implement caching layer for repository" → Decorator
/// Q: "Add metrics collection to services" → Decorator
/// Q: "How to combine multiple cross-cutting concerns?" → Decorator
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Decorators are stateless - safe for multi-instance deployment
/// • Each pod can have its own decorator chain
/// • Decorators wrap service calls (stateless transformation)
/// • Metrics decorators aggregate per-instance (use external metrics store)
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Decorators add logging before/after service calls
/// • Metrics: Decorators record execution time, success/failure counts
/// • Tracing: Decorators add trace spans for decorated operations
/// • Audit: Decorators log operations for compliance
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - mock decorated service, verify decorator behavior
/// • Test decorator chain independently
/// • Test each decorator in isolation
/// • Test decorator ordering and composition
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • Cross-Cutting Concerns: Logging, metrics, caching, retries
/// • Feature Composition: Combining multiple features dynamically
/// • Runtime Behavior: Adding behavior without modifying classes
/// • Aspect-Oriented: Separating concerns from business logic
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 8/10
/// ========================================================================
/// Very common in interviews, especially for cross-cutting concerns.
/// Important for understanding how to add features without modifying existing code.
/// Frequently used in enterprise applications.
/// </summary>
[ApiController]
[Route("api/patterns/decorator")]
public class DecoratorController : ControllerBase
{
    private readonly DecoratorScenario _scenario;
    
    public DecoratorController(DecoratorScenario scenario)
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
