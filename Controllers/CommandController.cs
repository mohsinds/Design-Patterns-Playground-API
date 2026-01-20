using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._06_Command.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Command pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Encapsulates a request as an object, allowing parameterization, queuing, 
/// logging, and undo operations.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Need to queue, log, or undo operations
/// • Want to parameterize objects with operations
/// • Need to support macro operations (command sequences)
/// • Want to decouple invoker from receiver
/// • Need audit trail or transaction support
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use for simple method calls (over-engineering)
/// • DO NOT use when undo is not needed and no queuing required
/// • Avoid when operations are stateless and don't need logging
/// • Don't use if you only need synchronous execution
/// • Anti-pattern: Command for every method call
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Order Management: Place, cancel, replace orders with undo and audit
/// 2. Transaction Processing: Commands for financial transactions with rollback
/// 3. Workflow Engines: Commands for business process steps
/// 4. Undo/Redo Systems: Trading platform order cancellation and redo
/// 5. Audit Logging: All operations logged as commands for compliance
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to implement undo/redo functionality?" → Command
/// Q: "Design a system that queues operations" → Command
/// Q: "How to log all operations for audit?" → Command
/// Q: "Support transaction rollback in application" → Command
/// Q: "Decouple operation execution from invocation" → Command
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Commands can be queued in distributed message queue (Kafka, RabbitMQ)
/// • Command handlers should be idempotent (handle duplicate commands)
/// • Use distributed locks for commands that modify shared state
/// • Command IDs should be globally unique (UUID)
/// • Consider command outbox pattern for reliable delivery
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Log all command executions, failures, retries
/// • Metrics: Track command execution times, success/failure rates
/// • Tracing: Trace spans for command execution with command ID
/// • Audit: All commands logged for compliance (SOX, PCI-DSS)
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - create command, execute, verify result
/// • Test undo functionality independently
/// • Test command queuing and ordering
/// • Mock command handlers for testing invokers
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • Undoable Operations: Operations that need rollback capability
/// • Queued Operations: Operations that need to be queued and processed later
/// • Auditable Operations: Operations that must be logged for compliance
/// • Transactional Operations: Operations that need atomic execution
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 9/10
/// ========================================================================
/// Very common in interviews, especially for system design. Critical for understanding
/// undo/redo, queuing, and audit requirements. Frequently asked in FinTech interviews.
/// </summary>
[ApiController]
[Route("api/patterns/command")]
public class CommandController : ControllerBase
{
    private readonly CommandScenario _scenario;
    
    public CommandController(CommandScenario scenario)
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
