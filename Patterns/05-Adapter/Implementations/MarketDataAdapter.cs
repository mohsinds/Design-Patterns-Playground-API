using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._05_Adapter.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._05_Adapter.Implementations;

/// <summary>
/// Adapter that converts legacy market data provider to modern interface.
/// Adapter pattern: allows incompatible interfaces to work together.
/// Thread-safety: Stateless adapter, safe for concurrent use.
/// </summary>
public class MarketDataAdapter : IModernMarketDataProvider
{
    private readonly ILegacyMarketDataProvider _legacyProvider;
    private readonly ILogger<MarketDataAdapter> _logger;
    
    public MarketDataAdapter(
        ILegacyMarketDataProvider legacyProvider,
        ILogger<MarketDataAdapter> logger)
    {
        _legacyProvider = legacyProvider;
        _logger = logger;
    }
    
    public async Task<Quote> GetQuoteAsync(string symbol, CancellationToken cancellationToken = default)
    {
        // Adapt legacy synchronous call to async
        var legacyQuote = await Task.Run(() => _legacyProvider.GetQuoteLegacy(symbol), cancellationToken);
        
        // Convert legacy format to modern format
        var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(legacyQuote.timestamp).DateTime;
        
        return new Quote(
            Symbol: legacyQuote.symbol,
            Bid: (decimal)legacyQuote.bid,
            Ask: (decimal)legacyQuote.ask,
            Last: (decimal)((legacyQuote.bid + legacyQuote.ask) / 2),
            Timestamp: timestamp
        );
    }
}
