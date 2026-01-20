using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._15_ChainOfResponsibility.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Chain of Responsibility pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Passes requests along a chain of handlers. Each handler decides either to 
/// process the request or pass it to the next handler in the chain.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Multiple objects can handle a request
/// • Want to decouple sender from receiver
/// • Need dynamic handler selection
/// • Want to add/remove handlers flexibly
/// • Need processing pipeline (validation, transformation, etc.)
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use when request must be handled by specific handler
/// • DO NOT use for simple sequential processing (use pipeline)
/// • Avoid when handler order doesn't matter
/// • Don't use if chain becomes too complex
/// • Anti-pattern: Chain where every handler processes every request
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Validation Pipeline: Basic → Risk → Account validation chain
/// 2. Request Processing: Authentication → Authorization → Rate Limit → Process
/// 3. Error Handling: Try handler 1, if fails try handler 2, etc.
/// 4. Middleware Pipeline: ASP.NET Core middleware chain
/// 5. Fraud Detection: Multiple fraud checks in sequence
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to process requests through multiple handlers?" → Chain of Responsibility
/// Q: "Design a validation pipeline" → Chain of Responsibility
/// Q: "Implement middleware-like processing" → Chain of Responsibility
/// Q: "How to add/remove processing steps dynamically?" → Chain of Responsibility
/// Q: "Process requests through multiple validators" → Chain of Responsibility
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Chain handlers are stateless - safe for multi-instance
/// • Each pod can have its own chain instance
/// • Request flows through chain (stateless processing)
/// • No shared state between chain instances
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Log each handler in chain, processing decisions
/// • Metrics: Track handler execution times, chain success/failure rates
/// • Tracing: Trace spans for each handler in chain
/// • Audit: Log chain processing for compliance (validation steps)
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - test each handler independently
/// • Test chain ordering and flow
/// • Test handler selection logic
/// • Mock handlers for testing chain behavior
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • Processing Pipelines: Sequential processing through multiple handlers
/// • Validation Chains: Multiple validators processing requests
/// • Middleware: Request processing through middleware chain
/// • Dynamic Handler Selection: Selecting handlers at runtime
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 7/10
/// ========================================================================
/// Common in interviews, especially for middleware and pipeline questions.
/// Important for understanding how to process requests through multiple handlers.
/// Frequently used in web frameworks (ASP.NET Core middleware).
/// </summary>
[ApiController]
[Route("api/patterns/chain-of-responsibility")]
public class ChainOfResponsibilityController : ControllerBase
{
    private readonly ChainOfResponsibilityScenario _scenario;
    
    public ChainOfResponsibilityController(ChainOfResponsibilityScenario scenario)
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
