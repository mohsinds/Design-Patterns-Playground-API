using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._01_Singleton.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Singleton pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Ensures a class has only one instance per application instance and provides 
/// global access to that instance.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Single shared resource (configuration, cache, connection pool)
/// • Expensive object creation (database connections, file handles)
/// • Global state that must be consistent across the application
/// • Logging, metrics collection, or audit services
/// • Thread-safe in-memory caches per application instance
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use for distributed coordination (each pod has its own singleton)
/// • DO NOT use when you need multiple instances with different configurations
/// • DO NOT use for stateless services (use DI scoped/transient instead)
/// • Avoid for testability - singletons are harder to mock and test
/// • Anti-pattern: Using singleton to avoid passing dependencies
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Configuration Service: Centralized app settings, feature flags, environment configs
/// 2. Connection Pool Manager: Database connection pooling, Redis connection management
/// 3. Metrics Collector: Application-wide metrics aggregation (Prometheus, StatsD)
/// 4. Audit Logger: Centralized audit trail service for compliance (SOX, PCI-DSS)
/// 5. Rate Limiter: Global rate limiting state per application instance
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How would you ensure only one configuration manager exists?" → Singleton
/// Q: "Design a connection pool that's shared across the app" → Singleton
/// Q: "How to implement a global metrics collector?" → Singleton
/// Q: "What pattern ensures one instance of a logger?" → Singleton
/// Q: "How to share a cache across multiple services in one app?" → Singleton
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Each pod/container has its own singleton instance
/// • NOT suitable for distributed coordination or shared state across pods
/// • Use database constraints, optimistic concurrency (rowversion), or distributed locks (Redis) instead
/// • Singleton is per-process, not per-cluster
/// • Example: 3 pods = 3 separate singleton instances
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Singleton logger instance shared across all components
/// • Metrics: Singleton metrics collector aggregates counters/gauges per instance
/// • Tracing: Singleton tracer manages trace context propagation
/// • Thread-safe logging is critical for singleton loggers
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Use Dependency Injection with singleton lifetime instead of static singletons
/// • Allows mocking via interface injection
/// • Reset state between tests using DI container recreation
/// • Avoid static singletons - they make testing difficult
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • Resource Management: Connection pools, file handles, memory caches
/// • Configuration: App settings, feature flags, environment variables
/// • Cross-cutting Concerns: Logging, metrics, auditing, rate limiting
/// • Stateful Services: In-memory state that must persist for app lifetime
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 7/10
/// ========================================================================
/// Common in interviews, but often misunderstood. Critical to explain distributed 
/// system limitations. Frequently asked about DI singleton vs. static singleton.
/// </summary>
[ApiController]
[Route("api/patterns/singleton")]
public class SingletonController : ControllerBase
{
    private readonly SingletonScenario _scenario;
    private readonly ILogger<SingletonController> _logger;
    
    public SingletonController(
        SingletonScenario scenario,
        ILogger<SingletonController> logger)
    {
        _scenario = scenario;
        _logger = logger;
    }
    
    /// <summary>
    /// Demo endpoint: demonstrates singleton pattern behavior.
    /// </summary>
    [HttpGet("demo")]
    [ProducesResponseType(typeof(PatternDemoResponse), 200)]
    public IActionResult Demo()
    {
        var response = _scenario.RunDemo();
        return Ok(response);
    }
    
    /// <summary>
    /// Test endpoint: runs deterministic tests for singleton pattern.
    /// </summary>
    [HttpGet("test")]
    [ProducesResponseType(typeof(PatternTestResponse), 200)]
    public IActionResult Test()
    {
        var response = _scenario.RunTest();
        return Ok(response);
    }
}
