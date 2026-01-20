using DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Contracts;

namespace DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Implementations;

/// <summary>
/// Stripe payment provider implementation.
/// Strategy pattern: Implements payment processing algorithm for Stripe payment gateway.
/// </summary>
public sealed class StripePaymentProvider : IPaymentProvider
{
    private readonly ILogger<StripePaymentProvider> _logger;
    private static readonly Random _random = new(42); // Deterministic seed for testing
    
    /// <summary>
    /// Initializes a new instance of the <see cref="StripePaymentProvider"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging operations.</param>
    public StripePaymentProvider(ILogger<StripePaymentProvider> logger)
    {
        _logger = logger;
    }
    
    /// <inheritdoc />
    public string ProviderKey => "stripe";
    
    /// <inheritdoc />
    public decimal MinimumAmount => 1.00m;
    
    /// <inheritdoc />
    public IReadOnlyList<string> SupportedCurrencies => new[] { "USD", "EUR", "GBP" };
    
    /// <inheritdoc />
    public bool ValidatePayment(decimal amount, string currency)
    {
        if (amount < MinimumAmount)
        {
            _logger.LogWarning("Payment amount {Amount} is below minimum {MinimumAmount} for provider {Provider}",
                amount, MinimumAmount, ProviderKey);
            return false;
        }
        
        if (!SupportedCurrencies.Contains(currency, StringComparer.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Currency {Currency} is not supported by provider {Provider}",
                currency, ProviderKey);
            return false;
        }
        
        return true;
    }
    
    /// <inheritdoc />
    public async Task<PaymentResult> ProcessPaymentAsync(
        decimal amount,
        string currency,
        string customerEmail,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing payment via Stripe: Amount={Amount}, Currency={Currency}, Email={Email}",
            amount, currency, customerEmail);
        
        // Simulate network delay
        await Task.Delay(50, cancellationToken);
        
        // Simulate 95% success rate
        var success = _random.NextDouble() > 0.05;
        
        var transactionId = $"stripe_txn_{Guid.NewGuid():N}";
        
        if (success)
        {
            _logger.LogInformation("Stripe payment successful: TransactionId={TransactionId}", transactionId);
            return new PaymentResult
            {
                TransactionId = transactionId,
                Status = "Success",
                ProviderUsed = ProviderKey,
                ProcessedAt = DateTime.UtcNow,
                Message = "Payment processed successfully via Stripe"
            };
        }
        
        _logger.LogWarning("Stripe payment failed: TransactionId={TransactionId}", transactionId);
        return new PaymentResult
        {
            TransactionId = transactionId,
            Status = "Failed",
            ProviderUsed = ProviderKey,
            ProcessedAt = DateTime.UtcNow,
            Message = "Payment processing failed"
        };
    }
}
