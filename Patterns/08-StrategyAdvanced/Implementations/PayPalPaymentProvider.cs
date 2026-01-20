using DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Contracts;

namespace DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Implementations;

/// <summary>
/// PayPal payment provider implementation.
/// Strategy pattern: Implements payment processing algorithm for PayPal payment gateway.
/// </summary>
public sealed class PayPalPaymentProvider : IPaymentProvider
{
    private readonly ILogger<PayPalPaymentProvider> _logger;
    private static readonly Random _random = new(43); // Different seed for variety
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PayPalPaymentProvider"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging operations.</param>
    public PayPalPaymentProvider(ILogger<PayPalPaymentProvider> logger)
    {
        _logger = logger;
    }
    
    /// <inheritdoc />
    public string ProviderKey => "paypal";
    
    /// <inheritdoc />
    public decimal MinimumAmount => 0.50m;
    
    /// <inheritdoc />
    public IReadOnlyList<string> SupportedCurrencies => new[] { "USD", "EUR", "CAD" };
    
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
        _logger.LogInformation("Processing payment via PayPal: Amount={Amount}, Currency={Currency}, Email={Email}",
            amount, currency, customerEmail);
        
        // Simulate network delay
        await Task.Delay(80, cancellationToken);
        
        // Simulate 90% success rate
        var success = _random.NextDouble() > 0.10;
        
        var transactionId = $"paypal_txn_{Guid.NewGuid():N}";
        
        if (success)
        {
            _logger.LogInformation("PayPal payment successful: TransactionId={TransactionId}", transactionId);
            return new PaymentResult
            {
                TransactionId = transactionId,
                Status = "Success",
                ProviderUsed = ProviderKey,
                ProcessedAt = DateTime.UtcNow,
                Message = "Payment processed successfully via PayPal"
            };
        }
        
        _logger.LogWarning("PayPal payment failed: TransactionId={TransactionId}", transactionId);
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
