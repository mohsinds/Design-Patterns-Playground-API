namespace DesignPatterns.Playground.Api.Patterns._01_Singleton.Contracts;

/// <summary>
/// Configuration service interface.
/// Singleton pattern: ensures single instance manages global configuration.
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Get a configuration value by key.
    /// </summary>
    string GetValue(string key);
    
    /// <summary>
    /// Get the instance ID (for demo purposes to show singleton behavior).
    /// </summary>
    string InstanceId { get; }
    
    /// <summary>
    /// Get access count (for demo purposes).
    /// </summary>
    int AccessCount { get; }
}
