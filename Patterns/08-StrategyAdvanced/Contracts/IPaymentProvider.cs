namespace DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Contracts;

/// <summary>
/// Payment provider interface for strategy pattern with dynamic provider selection.
/// Strategy pattern: Each provider implements a different payment processing algorithm.
/// </summary>
public interface IPaymentProvider
{
    /// <summary>
    /// Gets the unique key identifier for this payment provider.
    /// </summary>
    string ProviderKey { get; }
    
    /// <summary>
    /// Gets the minimum amount required for payments through this provider.
    /// </summary>
    decimal MinimumAmount { get; }
    
    /// <summary>
    /// Gets the list of currencies supported by this provider.
    /// </summary>
    IReadOnlyList<string> SupportedCurrencies { get; }
    
    /// <summary>
    /// Validates a payment request before processing.
    /// </summary>
    /// <param name="amount">The payment amount.</param>
    /// <param name="currency">The payment currency.</param>
    /// <returns>True if the payment request is valid; otherwise, false.</returns>
    bool ValidatePayment(decimal amount, string currency);
    
    /// <summary>
    /// Processes a payment asynchronously.
    /// </summary>
    /// <param name="amount">The payment amount.</param>
    /// <param name="currency">The payment currency.</param>
    /// <param name="customerEmail">The customer's email address.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the payment result.</returns>
    Task<PaymentResult> ProcessPaymentAsync(
        decimal amount,
        string currency,
        string customerEmail,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents the result of a payment processing operation.
/// </summary>
public record PaymentResult
{
    /// <summary>
    /// Gets or sets the unique transaction identifier.
    /// </summary>
    public string TransactionId { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the status of the payment (e.g., "Success", "Failed").
    /// </summary>
    public string Status { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the provider that was used to process the payment.
    /// </summary>
    public string ProviderUsed { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the timestamp when the payment was processed.
    /// </summary>
    public DateTime ProcessedAt { get; init; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets an optional message describing the payment result.
    /// </summary>
    public string? Message { get; init; }
}
