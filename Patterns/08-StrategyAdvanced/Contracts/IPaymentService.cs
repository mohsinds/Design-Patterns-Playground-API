namespace DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Contracts;

/// <summary>
/// Payment service interface for processing payments using dynamically selected providers.
/// Strategy pattern: Delegates payment processing to the appropriate provider strategy.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Processes a payment using the specified provider.
    /// </summary>
    /// <param name="request">The payment request containing amount, currency, provider key, and customer email.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the payment result.</returns>
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets information about all available payment providers.
    /// </summary>
    /// <returns>A collection of provider information.</returns>
    IEnumerable<ProviderInfo> GetAvailableProviders();
}

/// <summary>
/// Represents a payment request.
/// </summary>
public record PaymentRequest
{
    /// <summary>
    /// Gets or sets the payment amount.
    /// </summary>
    public decimal Amount { get; init; }
    
    /// <summary>
    /// Gets or sets the payment currency code (e.g., "USD", "EUR").
    /// </summary>
    public string Currency { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the unique key identifier of the payment provider to use.
    /// </summary>
    public string ProviderKey { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer's email address.
    /// </summary>
    public string CustomerEmail { get; init; } = string.Empty;
}

/// <summary>
/// Represents information about a payment provider.
/// </summary>
public record ProviderInfo
{
    /// <summary>
    /// Gets or sets the unique key identifier of the provider.
    /// </summary>
    public string Key { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the minimum amount required for payments through this provider.
    /// </summary>
    public decimal MinimumAmount { get; init; }
    
    /// <summary>
    /// Gets or sets the list of currencies supported by this provider.
    /// </summary>
    public IReadOnlyList<string> SupportedCurrencies { get; init; } = Array.Empty<string>();
}
