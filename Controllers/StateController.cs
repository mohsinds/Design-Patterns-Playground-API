using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._13_State.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating State pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Allows an object to alter its behavior when its internal state changes, 
/// appearing as if the object changed its class.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Object behavior depends on its state
/// • Have many conditional statements based on state
/// • State transitions are well-defined
/// • Want to encapsulate state-specific behavior
/// • Need to prevent invalid state transitions
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use for simple state flags (use enum)
/// • DO NOT use when state transitions are not well-defined
/// • Avoid when you only have 2-3 states
/// • Don't use if state behavior is trivial
/// • Anti-pattern: State pattern for every boolean flag
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Order Lifecycle: Pending → Placed → Filled/Cancelled state transitions
/// 2. Payment States: Initiated → Processing → Completed/Failed
/// 3. Account States: Active → Suspended → Closed with different behaviors
/// 4. Workflow States: Draft → Review → Approved → Published
/// 5. Trade States: New → Partially Filled → Filled with different validations
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to handle order lifecycle states?" → State
/// Q: "Design a state machine for payment processing" → State
/// Q: "How to prevent invalid state transitions?" → State
/// Q: "Encapsulate state-specific behavior" → State
/// Q: "Replace state-based conditionals with objects" → State
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • State objects are stateless - safe for multi-instance
/// • State transitions stored in database (not in-memory)
/// • Use optimistic concurrency for state transitions
/// • State validation prevents invalid transitions across pods
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Log state transitions, invalid transition attempts
/// • Metrics: Track state distribution, transition counts, invalid attempts
/// • Tracing: Trace spans for state transitions
/// • Audit: Log all state transitions for compliance (order state changes)
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - test each state independently
/// • Test state transitions and validations
/// • Test invalid transition prevention
/// • Test state-specific behavior
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • State Machines: Well-defined state transitions with validation
/// • Lifecycle Management: Object lifecycles with state-specific behavior
/// • Workflow States: Business process states with different behaviors
/// • Conditional Behavior: Behavior that depends on object state
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 8/10
/// ========================================================================
/// Very common in interviews, especially for state machine and lifecycle questions.
/// Important for understanding how to manage state transitions and prevent invalid states.
/// Frequently used in enterprise applications for business process management.
/// </summary>
[ApiController]
[Route("api/patterns/state")]
public class StateController : ControllerBase
{
    private readonly StateScenario _scenario;
    
    public StateController(StateScenario scenario)
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
