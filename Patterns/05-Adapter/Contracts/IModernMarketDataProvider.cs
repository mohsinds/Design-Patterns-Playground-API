using DesignPatterns.Playground.Api.Domain;

namespace DesignPatterns.Playground.Api.Patterns._05_Adapter.Contracts;

/// <summary>
/// Modern market data provider interface.
/// </summary>
public interface IModernMarketDataProvider
{
    /// <summary>
    /// Get quote in modern format.
    /// </summary>
    Task<Quote> GetQuoteAsync(string symbol, CancellationToken cancellationToken = default);
}
