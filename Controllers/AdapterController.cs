using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._05_Adapter.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Adapter pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Allows incompatible interfaces to work together by wrapping an object with 
/// an adapter that translates calls to the expected interface.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Integrating third-party libraries with incompatible interfaces
/// • Wrapping legacy systems to work with modern code
/// • Converting synchronous APIs to asynchronous
/// • Adapting data formats (XML to JSON, tuples to objects)
/// • Making incompatible classes work together
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use when you can modify the source interface directly
/// • DO NOT use for simple type conversions (use mappers)
/// • Avoid when adapter adds significant performance overhead
/// • Don't use if interfaces are already compatible
/// • Anti-pattern: Adapter that just passes through without transformation
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Legacy System Integration: Wrap old SOAP APIs to work with REST clients
/// 2. Market Data Adapters: Convert legacy quote format to modern async interface
/// 3. Payment Gateway Adapters: Unify different payment provider interfaces
/// 4. Database Adapters: Wrap different ORMs (Entity Framework to Dapper)
/// 5. Message Queue Adapters: Convert between RabbitMQ, Kafka, Azure Service Bus formats
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to make a legacy synchronous API work with async code?" → Adapter
/// Q: "Integrate incompatible third-party library" → Adapter
/// Q: "Convert between different data formats" → Adapter
/// Q: "Wrap old system to work with new interface" → Adapter
/// Q: "How to make two incompatible classes work together?" → Adapter
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Adapters are stateless - safe for multi-instance deployment
/// • Each pod can have its own adapter instances
/// • No shared state between adapter instances
/// • Adapters typically wrap external services (stateless transformation)
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Log adapter transformations, format conversions, errors
/// • Metrics: Track adapter usage, conversion success/failure rates
/// • Tracing: Trace spans for adapter transformations
/// • Audit: Log what was adapted for compliance (legacy system calls)
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - mock adapted object, verify adapter calls
/// • Test transformation logic independently
/// • Test error handling when adapted object fails
/// • Test async/sync conversions
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • Legacy Integration: Wrapping old systems for modern interfaces
/// • Interface Conversion: Converting between incompatible interfaces
/// • Format Transformation: Converting data formats, protocols
/// • Third-Party Integration: Adapting external library interfaces
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 7/10
/// ========================================================================
/// Common in interviews, especially for system integration questions.
/// Important for understanding how to work with legacy systems and third-party libraries.
/// </summary>
[ApiController]
[Route("api/patterns/adapter")]
public class AdapterController : ControllerBase
{
    private readonly AdapterScenario _scenario;
    
    public AdapterController(AdapterScenario scenario)
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
