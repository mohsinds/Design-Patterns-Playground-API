namespace DesignPatterns.Playground.Api.Patterns._14_Prototype.Contracts;

/// <summary>
/// Prototype interface for cloning.
/// Prototype pattern: enables creating copies of objects (useful for snapshots, backtests).
/// </summary>
public interface IPrototype<out T>
{
    /// <summary>
    /// Create a deep copy of the object.
    /// </summary>
    T Clone();
}
