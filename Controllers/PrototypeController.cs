using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._14_Prototype.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Prototype pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Creates objects by cloning existing instances rather than creating new ones 
/// from scratch. Useful when object creation is expensive.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Object creation is expensive (database queries, network calls)
/// • Need to create objects similar to existing ones
/// • Want to avoid subclassing for object creation
/// • Need to create objects at runtime based on existing instances
/// • Want to preserve object state for snapshots
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use for simple object creation (use constructors)
/// • DO NOT use when objects are cheap to create
/// • Avoid when deep copying is not needed (use shallow copy)
/// • Don't use if cloning adds unnecessary complexity
/// • Anti-pattern: Prototype for every object creation
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Backtesting: Clone portfolio snapshots for different strategy tests
/// 2. Order Templates: Clone order templates with different parameters
/// 3. Configuration Snapshots: Clone configs for different environments
/// 4. Report Templates: Clone report templates with different data
/// 5. State Snapshots: Clone object state for undo/redo or audit
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to create objects similar to existing ones?" → Prototype
/// Q: "Design a backtesting system with portfolio snapshots" → Prototype
/// Q: "How to avoid expensive object creation?" → Prototype
/// Q: "Create object snapshots for undo/redo" → Prototype
/// Q: "Clone complex objects efficiently" → Prototype
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Prototypes are cloned per-instance (not shared)
/// • Each pod clones objects independently
/// • Deep cloning ensures independence between clones
/// • No shared state between prototype instances
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Log prototype cloning operations, clone creation
/// • Metrics: Track prototype usage, clone creation times
/// • Tracing: Trace spans for cloning operations
/// • Audit: Log what was cloned for compliance (snapshot creation)
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - create prototype, clone, verify independence
/// • Test deep vs shallow cloning
/// • Test clone modifications don't affect original
/// • Test cloning performance
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • Expensive Creation: Avoiding expensive object creation
/// • Snapshot Creation: Creating snapshots of object state
/// • Template Cloning: Cloning templates with modifications
/// • State Preservation: Preserving object state for later use
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 5/10
/// ========================================================================
/// Less common in interviews, but important for specific scenarios.
/// Useful for understanding object cloning and snapshot patterns.
/// More relevant for specific domains (backtesting, configuration management).
/// </summary>
[ApiController]
[Route("api/patterns/prototype")]
public class PrototypeController : ControllerBase
{
    private readonly PrototypeScenario _scenario;
    
    public PrototypeController(PrototypeScenario scenario)
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
