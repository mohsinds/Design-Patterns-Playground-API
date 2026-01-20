using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._11_Repository.Scenarios;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for demonstrating Repository pattern.
/// 
/// ========================================================================
/// PATTERN DEFINITION
/// ========================================================================
/// Mediates between domain and data mapping layers, providing an abstraction 
/// over data access. Enables testing and decouples business logic from persistence.
/// 
/// ========================================================================
/// WHEN TO USE
/// ========================================================================
/// • Need to abstract data access from business logic
/// • Want to make code testable (mock repositories)
/// • Need to support multiple data sources
/// • Want to centralize data access logic
/// • Need to implement Unit of Work for transactions
/// 
/// ========================================================================
/// WHEN NOT TO USE / COMMON TRAPS
/// ========================================================================
/// • DO NOT use for simple CRUD (use ORM directly)
/// • DO NOT use when you need direct SQL access everywhere
/// • Avoid when repository becomes just a pass-through
/// • Don't use if you only have one entity type
/// • Anti-pattern: Generic repository for everything (loses type safety)
/// 
/// ========================================================================
/// ENTERPRISE/FINTECH USE-CASES
/// ========================================================================
/// 1. Order Repository: Abstract order persistence, support in-memory for testing
/// 2. Account Repository: Centralize account data access, support caching
/// 3. Transaction Repository: Abstract transaction persistence, support audit
/// 4. Market Data Repository: Abstract market data storage, support multiple sources
/// 5. Audit Repository: Centralize audit log persistence, support compliance queries
/// 
/// ========================================================================
/// INTERVIEW QUESTION EXAMPLES
/// ========================================================================
/// Q: "How to make data access testable?" → Repository
/// Q: "Abstract database access from business logic" → Repository
/// Q: "Support multiple data sources" → Repository
/// Q: "How to implement Unit of Work pattern?" → Repository
/// Q: "Decouple domain from persistence layer" → Repository
/// 
/// ========================================================================
/// MULTI-INSTANCE DEPLOYMENT (KUBERNETES)
/// ========================================================================
/// • Repositories access shared database (not in-memory)
/// • Use optimistic concurrency (rowversion) for concurrent updates
/// • Repository instances are stateless - safe for multi-instance
/// • Unit of Work coordinates transactions across repositories
/// • Consider connection pooling for database access
/// 
/// ========================================================================
/// LOGGING/METRICS/TRACING INTERACTION
/// ========================================================================
/// • Logging: Log repository operations, queries, errors
/// • Metrics: Track repository call counts, query execution times
/// • Tracing: Trace spans for database operations
/// • Audit: Log data access for compliance (who accessed what)
/// 
/// ========================================================================
/// UNIT TEST FRIENDLINESS
/// ========================================================================
/// • Easy to test - mock repository interface, test business logic
/// • Use in-memory repository for integration tests
/// • Test repository implementation independently
/// • Test Unit of Work transaction coordination
/// 
/// ========================================================================
/// USE CASE TYPES
/// ========================================================================
/// • Data Access Abstraction: Hiding persistence details from business logic
/// • Testability: Enabling unit testing with mock repositories
/// • Multi-Source Support: Supporting different data sources
/// • Transaction Management: Coordinating transactions with Unit of Work
/// 
/// ========================================================================
/// IMPORTANCE SCORE: 8/10
/// ========================================================================
/// Very common in interviews, especially for data access and testing questions.
/// Important for understanding how to abstract persistence and enable testability.
/// Frequently used in enterprise applications.
/// </summary>
[ApiController]
[Route("api/patterns/repository")]
public class RepositoryController : ControllerBase
{
    private readonly RepositoryScenario _scenario;
    
    public RepositoryController(RepositoryScenario scenario)
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
