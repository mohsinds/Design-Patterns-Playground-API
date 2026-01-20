using DesignPatterns.Playground.Api.Patterns._01_Singleton.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._01_Singleton.Implementations;

/// <summary>
/// Singleton configuration service implementation.
/// 
/// IMPORTANT NOTES ON SINGLETON IN DISTRIBUTED SYSTEMS:
/// ====================================================
/// 1. Singleton in .NET DI (AddSingleton) only ensures ONE instance PER APPLICATION INSTANCE.
///    - In Kubernetes with multiple pods, each pod has its own singleton instance.
///    - This does NOT solve distributed concurrency problems.
/// 
/// 2. For distributed systems, use:
///    - Database constraints (UNIQUE constraints, rowversion/optimistic concurrency)
///    - Distributed locks (Redis, etc.) - but beware of deadlocks and performance
///    - Transactional outbox pattern for event publishing
///    - Idempotency keys for operations
/// 
/// 3. Singleton is useful for:
///    - In-memory caches (per-instance)
///    - Configuration that doesn't need cross-instance consistency
///    - Expensive resource initialization (connection pools, etc.)
/// 
/// Thread-safety: This implementation is thread-safe when used via DI singleton lifetime.
/// </summary>
public class ConfigurationService : IConfigurationService
{
    private static int _instanceCounter = 0;
    private readonly Dictionary<string, string> _config;
    private int _accessCount = 0;
    private readonly string _instanceId;
    
    // Thread-safe increment using Interlocked
    private static int GetNextInstanceNumber() => Interlocked.Increment(ref _instanceCounter);
    
    public ConfigurationService()
    {
        _instanceId = $"ConfigService-{GetNextInstanceNumber()}-{Guid.NewGuid():N}";
        _config = new Dictionary<string, string>
        {
            ["TradingApiUrl"] = "https://api.trading.example.com",
            ["RiskCheckEnabled"] = "true",
            ["MaxOrderSize"] = "1000000",
            ["DefaultCurrency"] = "USD",
            ["KafkaBootstrapServers"] = "localhost:9092"
        };
    }
    
    public string GetValue(string key)
    {
        Interlocked.Increment(ref _accessCount);
        return _config.TryGetValue(key, out var value) ? value : string.Empty;
    }
    
    public string InstanceId => _instanceId;
    
    public int AccessCount => _accessCount;
}
