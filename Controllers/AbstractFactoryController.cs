using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._03_AbstractFactory.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Abstract Factory pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Provides an interface for creating families of related or dependent objects 
/// without specifying their concrete classes.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Need to create families of related objects
/// • Objects must work together (compatibility requirement)
/// • Want to hide implementation details of product families
/// • Need to switch between product families at runtime
/// • Products are platform-specific or provider-specific
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use for single product creation (use Factory Method)
/// • DO NOT use when products aren't related (over-engineering)
/// • Avoid when you only have one product family
/// • Don't use if products don't need to work together
/// • Anti-pattern: Abstract factory for unrelated objects
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Payment Gateway Families: Stripe (gateway + config + webhook handler) vs PayPal
/// 2. Database Providers: SQL Server (connection + command + adapter) vs PostgreSQL
/// 3. Cloud Providers: AWS (S3 + SQS + SNS) vs Azure (Blob + Queue + Service Bus)
/// 4. Trading Platforms: Interactive Brokers (API + data feed + order router) vs FIX
/// 5. UI Frameworks: Material (button + dialog + theme) vs Bootstrap components
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to create compatible payment gateway and its configuration?" → Abstract Factory
/// Q: "Design a system supporting multiple cloud providers" → Abstract Factory
/// Q: "Create families of related UI components" → Abstract Factory
/// Q: "How to ensure database objects work together?" → Abstract Factory
/// Q: "Switch between provider ecosystems at runtime" → Abstract Factory
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Factory instances are stateless - safe for multi-instance
/// • Each pod can use different factory (e.g., different cloud regions)
/// • Factory selection can be based on pod labels or config maps
/// • No shared state - each instance creates its own product families
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Log which factory family was selected and why
/// • Metrics: Track factory usage by family type, product creation counts
/// • Tracing: Trace spans for entire product family creation
/// • Audit: Log factory selection for compliance (which provider used)
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - inject factory interface, mock product families
/// • Test factory selection logic
/// • Mock entire product families for integration testing
/// • Test compatibility between products in a family
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • Provider Ecosystems: Payment gateways, cloud providers, database vendors
/// • Platform Abstraction: Cross-platform UI, OS-specific implementations
/// • Compatible Components: UI themes, database adapters, messaging systems
/// • Vendor Lock-in Prevention: Abstract away provider-specific implementations
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 7/10
/// ========================================================================
/// Common in interviews, especially for system design. Important for understanding
/// how to abstract provider ecosystems and create compatible object families.
/// </summary>
[ApiController]
[Route("api/patterns/abstract-factory")]
public class AbstractFactoryController : ControllerBase
{
    private readonly AbstractFactoryScenario _scenario;
    
    public AbstractFactoryController(AbstractFactoryScenario scenario)
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
