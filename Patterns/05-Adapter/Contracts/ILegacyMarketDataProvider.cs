namespace DesignPatterns.Playground.Api.Patterns._05_Adapter.Contracts;

/// <summary>
/// Legacy market data provider interface (incompatible with modern system).
/// Adapter pattern: adapts legacy interface to modern interface.
/// </summary>
public interface ILegacyMarketDataProvider
{
    /// <summary>
    /// Get quote in legacy format (returns tuple).
    /// </summary>
    (string symbol, double bid, double ask, long timestamp) GetQuoteLegacy(string symbol);
}
