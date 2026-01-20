namespace DesignPatterns.Playground.Api.Infrastructure;

/// <summary>
/// Metrics interface for observability hooks.
/// In production, this would integrate with Prometheus, Application Insights, etc.
/// </summary>
public interface IMetrics
{
    /// <summary>
    /// Increment a counter metric.
    /// </summary>
    void IncrementCounter(string name, Dictionary<string, string>? tags = null);
    
    /// <summary>
    /// Record a histogram/timing metric.
    /// </summary>
    void RecordDuration(string name, TimeSpan duration, Dictionary<string, string>? tags = null);
    
    /// <summary>
    /// Set a gauge value.
    /// </summary>
    void SetGauge(string name, double value, Dictionary<string, string>? tags = null);
}

/// <summary>
/// In-memory implementation for demo purposes.
/// </summary>
public class InMemoryMetrics : IMetrics
{
    private readonly ILogger<InMemoryMetrics> _logger;
    private readonly Dictionary<string, long> _counters = new();
    private readonly Dictionary<string, List<TimeSpan>> _durations = new();
    private readonly Dictionary<string, double> _gauges = new();
    
    public InMemoryMetrics(ILogger<InMemoryMetrics> logger)
    {
        _logger = logger;
    }
    
    public void IncrementCounter(string name, Dictionary<string, string>? tags = null)
    {
        var key = tags != null ? $"{name}[{string.Join(",", tags)}]" : name;
        _counters.TryGetValue(key, out var count);
        _counters[key] = count + 1;
        _logger.LogDebug("Counter {Name} incremented to {Count}", key, _counters[key]);
    }
    
    public void RecordDuration(string name, TimeSpan duration, Dictionary<string, string>? tags = null)
    {
        var key = tags != null ? $"{name}[{string.Join(",", tags)}]" : name;
        if (!_durations.ContainsKey(key))
            _durations[key] = new List<TimeSpan>();
        _durations[key].Add(duration);
        _logger.LogDebug("Duration {Name} recorded: {Duration}ms", key, duration.TotalMilliseconds);
    }
    
    public void SetGauge(string name, double value, Dictionary<string, string>? tags = null)
    {
        var key = tags != null ? $"{name}[{string.Join(",", tags)}]" : name;
        _gauges[key] = value;
        _logger.LogDebug("Gauge {Name} set to {Value}", key, value);
    }
    
    public Dictionary<string, object> GetSnapshot()
    {
        return new Dictionary<string, object>
        {
            ["counters"] = _counters,
            ["durations"] = _durations.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(d => d.TotalMilliseconds).ToList()),
            ["gauges"] = _gauges
        };
    }
}
