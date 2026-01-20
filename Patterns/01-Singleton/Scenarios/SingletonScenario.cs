using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._01_Singleton.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._01_Singleton.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Singleton pattern.
/// </summary>
public class SingletonScenario
{
    private readonly IConfigurationService _configService;
    private readonly ILogger<SingletonScenario> _logger;
    
    public SingletonScenario(
        IConfigurationService configService,
        ILogger<SingletonScenario> logger)
    {
        _configService = configService;
        _logger = logger;
    }
    
    /// <summary>
    /// Demo: Show singleton behavior - same instance ID across multiple calls.
    /// </summary>
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Access the service multiple times
        for (int i = 0; i < 5; i++)
        {
            var configValue = _configService.GetValue("TradingApiUrl");
            results.Add(new
            {
                Call = i + 1,
                InstanceId = _configService.InstanceId,
                ConfigValue = configValue,
                AccessCount = _configService.AccessCount
            });
        }
        
        return new PatternDemoResponse(
            Pattern: "Singleton",
            Description: "Demonstrates singleton pattern: same instance ID across multiple calls, shared state (access count).",
            Result: new
            {
                InstanceId = _configService.InstanceId,
                Calls = results,
                Note = "In a distributed system (Kubernetes), each pod would have its own singleton instance. " +
                       "Use database constraints, optimistic concurrency, or distributed locks for cross-instance coordination."
            },
            Metadata: new Dictionary<string, object>
            {
                ["ThreadSafe"] = true,
                ["ScalabilityNote"] = "Singleton per instance only; not suitable for distributed coordination"
            }
        );
    }
    
    /// <summary>
    /// Test: Verify singleton behavior.
    /// </summary>
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Check 1: Instance ID should remain constant
        var instanceId1 = _configService.InstanceId;
        var instanceId2 = _configService.InstanceId;
        checks.Add(new TestCheck(
            "Instance ID Consistency",
            instanceId1 == instanceId2,
            $"Instance IDs match: {instanceId1}"
        ));
        
        // Check 2: Access count should increment
        var accessCount1 = _configService.AccessCount;
        _configService.GetValue("TestKey");
        var accessCount2 = _configService.AccessCount;
        checks.Add(new TestCheck(
            "Access Count Increment",
            accessCount2 > accessCount1,
            $"Access count increased from {accessCount1} to {accessCount2}"
        ));
        
        // Check 3: Configuration values should be accessible
        var apiUrl = _configService.GetValue("TradingApiUrl");
        checks.Add(new TestCheck(
            "Configuration Access",
            !string.IsNullOrEmpty(apiUrl),
            $"Retrieved config value: {apiUrl}"
        ));
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Singleton",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
