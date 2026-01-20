using DesignPatterns.Playground.Api.Domain;

namespace DesignPatterns.Playground.Api.Patterns._10_Facade.Contracts;

/// <summary>
/// Trading facade interface.
/// Facade pattern: simplifies complex trading subsystem interactions.
/// </summary>
public interface ITradingFacade
{
    /// <summary>
    /// Place an order (handles validation, risk checks, persistence, events).
    /// </summary>
    Task<PlaceOrderResult> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Cancel an order (handles validation, persistence, events).
    /// </summary>
    Task<CancelOrderResult> CancelOrderAsync(CancelOrderRequest request, CancellationToken cancellationToken = default);
}

    /// <summary>
    /// Place order result.
    /// </summary>
    public record PlaceOrderResult(bool Success, Order? Order = null, List<string>? Errors = null);

/// <summary>
/// Cancel order result.
/// </summary>
public record CancelOrderResult(bool Success, string? ErrorMessage = null);
