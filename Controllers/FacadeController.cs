using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._10_Facade.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Facade pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Provides a unified interface to a set of interfaces in a subsystem, 
/// making it easier to use by hiding complexity.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Need to simplify complex subsystem interactions
/// • Want to provide a simple interface to complex system
/// • Need to decouple client from subsystem
/// • Want to reduce dependencies between layers
/// • Need to wrap legacy system with simpler API
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use when you need direct access to subsystem components
/// • DO NOT use if it adds unnecessary abstraction layer
/// • Avoid when subsystem is already simple
/// • Don't use if facade becomes too complex (anti-facade)
/// • Anti-pattern: Facade that just passes through without simplification
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Trading Facade: Simplifies order placement (validation + risk + repository + events)
/// 2. Payment Facade: Hides complexity of payment gateway, fraud check, ledger update
/// 3. Reporting Facade: Simplifies report generation (data fetch + transform + format)
/// 4. API Gateway: Simplifies microservices communication for clients
/// 5. Legacy System Wrapper: Wraps old system with modern REST API
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to simplify complex subsystem interactions?" → Facade
/// Q: "Design a simple API for complex trading system" → Facade
/// Q: "How to hide subsystem complexity from clients?" → Facade
/// Q: "Wrap legacy system with modern interface" → Facade
/// Q: "Simplify microservices communication" → Facade
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Facades are stateless - safe for multi-instance deployment
/// • Each pod can have its own facade instance
/// • Facade coordinates calls to subsystem (stateless orchestration)
/// • No shared state between facade instances
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Log facade operations, subsystem calls, errors
/// • Metrics: Track facade usage, subsystem call counts, latencies
/// • Tracing: Trace spans for facade operations and subsystem calls
/// • Audit: Log facade operations for compliance (trading operations)
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - mock subsystem components, test facade orchestration
/// • Test facade simplifies subsystem correctly
/// • Test error handling when subsystem fails
/// • Test facade coordinates multiple subsystem calls
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • System Simplification: Hiding complex subsystem behind simple interface
/// • API Design: Providing clean API over complex implementation
/// • Legacy Integration: Wrapping old systems with modern interface
/// • Layer Abstraction: Simplifying interactions between layers
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 7/10
/// ========================================================================
/// Common in interviews, especially for API design and system simplification.
/// Important for understanding how to hide complexity and provide clean interfaces.
/// </summary>
[ApiController]
[Route("api/patterns/facade")]
public class FacadeController : ControllerBase
{
    private readonly FacadeScenario _scenario;
    
    public FacadeController(FacadeScenario scenario)
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
