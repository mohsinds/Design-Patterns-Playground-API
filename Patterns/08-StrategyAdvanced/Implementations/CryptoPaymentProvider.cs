using DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Contracts;

namespace DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Implementations;

/// <summary>
/// Cryptocurrency payment provider implementation.
/// Strategy pattern: Implements payment processing algorithm for cryptocurrency payments.
/// </summary>
public sealed class CryptoPaymentProvider : IPaymentProvider
{
    private readonly ILogger<CryptoPaymentProvider> _logger;
    private static readonly Random _random = new(44); // Different seed for variety
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CryptoPaymentProvider"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging operations.</param>
    public CryptoPaymentProvider(ILogger<CryptoPaymentProvider> logger)
    {
        _logger = logger;
    }
    
    /// <inheritdoc />
    public string ProviderKey => "crypto";
    
    /// <inheritdoc />
    public decimal MinimumAmount => 10.00m;
    
    /// <inheritdoc />
    public IReadOnlyList<string> SupportedCurrencies => new[] { "BTC", "ETH", "USDT" };
    
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
        _logger.LogInformation("Processing payment via Crypto: Amount={Amount}, Currency={Currency}, Email={Email}",
            amount, currency, customerEmail);
        
        // Simulate blockchain confirmation delay
        await Task.Delay(200, cancellationToken);
        
        // Simulate 85% success rate (blockchain can have failures)
        var success = _random.NextDouble() > 0.15;
        
        var transactionId = $"crypto_txn_{Guid.NewGuid():N}";
        
        if (success)
        {
            _logger.LogInformation("Crypto payment successful: TransactionId={TransactionId}", transactionId);
            return new PaymentResult
            {
                TransactionId = transactionId,
                Status = "Success",
                ProviderUsed = ProviderKey,
                ProcessedAt = DateTime.UtcNow,
                Message = "Payment processed successfully via Cryptocurrency"
            };
        }
        
        _logger.LogWarning("Crypto payment failed: TransactionId={TransactionId}", transactionId);
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
