using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._08_Strategy.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Strategy pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Defines a family of algorithms, encapsulates each one, and makes them 
/// interchangeable. Strategy lets the algorithm vary independently from clients.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Multiple ways to perform a task (algorithms)
/// • Want to switch algorithms at runtime
/// • Need to avoid conditional statements (if/switch)
/// • Algorithms should be interchangeable
/// • Want to isolate algorithm implementation from usage
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use for simple conditional logic (use if/switch)
/// • DO NOT use when you only have one algorithm
/// • Avoid when algorithms are not interchangeable
/// • Don't use if algorithm selection is compile-time only
/// • Anti-pattern: Strategy for every conditional branch
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Pricing Strategies: Market price, limit price, VWAP, risk-adjusted pricing
/// 2. Risk Calculation: Different risk models (VaR, CVaR, stress testing)
/// 3. Routing Algorithms: Order routing strategies (smart order routing, TWAP, VWAP)
/// 4. Payment Processing: Different payment methods (credit card, ACH, wire transfer)
/// 5. Tax Calculation: Different tax strategies by jurisdiction
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to switch between different pricing algorithms?" → Strategy
/// Q: "Design a system with multiple calculation methods" → Strategy
/// Q: "How to avoid if/switch for algorithm selection?" → Strategy
/// Q: "Implement different routing algorithms" → Strategy
/// Q: "Support multiple payment processing methods" → Strategy
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Strategies are stateless - safe for multi-instance deployment
/// • Strategy selection can be based on request data or configuration
/// • Each pod can use different strategies independently
/// • No shared state between strategy instances
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Log which strategy was selected and why
/// • Metrics: Track strategy usage, performance by strategy type
/// • Tracing: Trace spans for strategy execution
/// • Audit: Log strategy selection for compliance (pricing decisions)
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - test each strategy independently
/// • Mock strategies for testing context/selector
/// • Test strategy selection logic
/// • Test strategy interchangeability
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • Algorithm Selection: Choosing between different algorithms at runtime
/// • Business Rules: Different rules for different scenarios
/// • Calculation Methods: Multiple ways to calculate the same thing
/// • Processing Strategies: Different ways to process data
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 10/10
/// ========================================================================
/// Most important pattern for interviews. Extremely common in system design questions.
/// Essential for understanding polymorphism and runtime algorithm selection.
/// Frequently asked in FinTech interviews for pricing, risk, and routing scenarios.
/// </summary>
[ApiController]
[Route("api/patterns/strategy")]
public class StrategyController : ControllerBase
{
    private readonly StrategyScenario _scenario;
    
    public StrategyController(StrategyScenario scenario)
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
