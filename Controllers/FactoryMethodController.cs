using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Factory Method pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Defines an interface for creating objects, but lets subclasses decide which 
/// class to instantiate. Encapsulates object creation logic.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Object creation logic is complex or conditional
/// • Need to decouple object creation from usage
/// • Want to support multiple product types with same interface
/// • Creation logic may change or extend frequently
/// • Need to centralize creation rules (validation, configuration)
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use for simple object creation (use constructors)
/// • DO NOT use when creation logic is trivial (over-engineering)
/// • Avoid when you need runtime type selection (use Abstract Factory)
/// • Don't use if you only have one product type
/// • Anti-pattern: Factory that just wraps "new" keyword
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Order Validators: Different validators for standard vs. large orders
/// 2. Risk Calculators: Factory creates appropriate risk model based on asset type
/// 3. Report Generators: PDF, Excel, CSV reports created via factory
/// 4. Payment Processors: Factory selects processor based on payment method
/// 5. Notification Channels: Email, SMS, Push notification factories
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to create different validators based on order type?" → Factory Method
/// Q: "Design a system that creates reports in different formats" → Factory Method
/// Q: "How to instantiate different risk calculators dynamically?" → Factory Method
/// Q: "Create objects based on configuration or runtime conditions" → Factory Method
/// Q: "How to decouple object creation from business logic?" → Factory Method
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Factory instances are stateless - safe for multi-instance deployment
/// • Each pod creates objects independently
/// • No shared state between factory instances
/// • Factory selection logic should be deterministic (same input = same output)
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Factory logs which product type was created and why
/// • Metrics: Track factory method calls, product creation counts by type
/// • Tracing: Trace spans for factory creation decisions
/// • Audit: Log factory selection criteria for compliance
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - inject factory interface, mock product creation
/// • Test factory selection logic independently
/// • Mock products returned by factory for testing consumers
/// • Test edge cases in factory decision logic
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • Conditional Creation: Objects created based on runtime conditions
/// • Type Selection: Choosing implementation based on input parameters
/// • Validation Logic: Creating validators based on data characteristics
/// • Configuration-Driven: Objects created from config files or settings
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 8/10
/// ========================================================================
/// Very common in interviews. Frequently asked about Factory vs. Abstract Factory.
/// Essential for understanding object creation patterns.
/// </summary>
[ApiController]
[Route("api/patterns/factory-method")]
public class FactoryMethodController : ControllerBase
{
    private readonly FactoryMethodScenario _scenario;
    
    public FactoryMethodController(FactoryMethodScenario scenario)
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
