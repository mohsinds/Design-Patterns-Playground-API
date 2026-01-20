using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._04_Builder.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Builder pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Separates the construction of a complex object from its representation, 
/// allowing the same construction process to create different representations.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Complex object construction with many optional parameters
/// • Need fluent/readable object construction API
/// • Want to validate object before construction
/// • Need to construct objects step-by-step
/// • Constructor has too many parameters (parameter object anti-pattern)
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use for simple objects (use constructors or object initializers)
/// • DO NOT use when all parameters are required (use constructor)
/// • Avoid when object construction is trivial
/// • Don't use if you only have one representation
/// • Anti-pattern: Builder for objects with 2-3 parameters
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Order Construction: Build orders with optional fields (limit price, stop loss, time in force)
/// 2. SQL Query Builder: Fluent API for building complex SQL queries
/// 3. HTTP Request Builder: Construct API requests with headers, body, auth
/// 4. Configuration Objects: Build complex configs with validation
/// 5. Report Generation: Build reports with sections, filters, formatting options
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to construct objects with many optional parameters?" → Builder
/// Q: "Design a fluent API for building SQL queries" → Builder
/// Q: "Create complex configuration objects step-by-step" → Builder
/// Q: "How to validate object before construction?" → Builder
/// Q: "Build HTTP requests with optional headers and body" → Builder
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Builder instances are stateless - safe for multi-instance
/// • Each request can use its own builder instance
/// • No shared state between builder instances
/// • Builders are typically scoped per request (transient or scoped)
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Log builder steps, validation failures, construction attempts
/// • Metrics: Track builder usage, construction success/failure rates
/// • Tracing: Trace spans for object construction process
/// • Audit: Log what was built for compliance (order details, configs)
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - create builder, set properties, verify result
/// • Test validation logic in Build() method
/// • Test fluent interface chaining
/// • Test different object representations from same builder
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • Complex Construction: Objects with many optional/conditional parameters
/// • Fluent APIs: Readable, chainable object construction
/// • Validation: Validate object state before construction
/// • Step-by-Step Building: Construct objects incrementally
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 8/10
/// ========================================================================
/// Very common in interviews. Frequently asked about Builder vs. constructor overloads.
/// Essential for understanding fluent APIs and complex object construction.
/// </summary>
[ApiController]
[Route("api/patterns/builder")]
public class BuilderController : ControllerBase
{
    private readonly BuilderScenario _scenario;
    
    public BuilderController(BuilderScenario scenario)
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
