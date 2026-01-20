namespace DesignPatterns.Playground.Api.Infrastructure;

/// <summary>
/// Payment gateway interface for processing payments.
/// Supports multiple providers (Stripe, PayPal, etc.) via adapter pattern.
/// </summary>
public interface IPaymentGateway
{
    /// <summary>
    /// Process a payment.
    /// </summary>
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get the gateway provider name.
    /// </summary>
    string ProviderName { get; }
}

/// <summary>
/// Payment request.
/// </summary>
public record PaymentRequest(
    string TransactionId,
    decimal Amount,
    string Currency,
    string AccountId,
    Dictionary<string, string>? Metadata = null
);

/// <summary>
/// Payment result.
/// </summary>
public record PaymentResult(
    bool Success,
    string TransactionId,
    string? ErrorMessage = null,
    DateTime ProcessedAt = default
)
{
    public DateTime ProcessedAt { get; init; } = ProcessedAt == default ? DateTime.UtcNow : ProcessedAt;
}

/// <summary>
/// Fake Stripe payment gateway implementation.
/// </summary>
public class FakeStripeGateway : IPaymentGateway
{
    private readonly ILogger<FakeStripeGateway> _logger;
    private static readonly Random _random = new(42); // Deterministic seed
    
    public FakeStripeGateway(ILogger<FakeStripeGateway> logger)
    {
        _logger = logger;
    }
    
    public string ProviderName => "Stripe";
    
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        // Simulate network delay
        await Task.Delay(50, cancellationToken);
        
        // Simulate 95% success rate
        var success = _random.NextDouble() > 0.05;
        
        if (success)
        {
            _logger.LogInformation("Stripe payment processed: {TransactionId}, Amount: {Amount} {Currency}",
                request.TransactionId, request.Amount, request.Currency);
            return new PaymentResult(true, request.TransactionId);
        }
        
        _logger.LogWarning("Stripe payment failed: {TransactionId}", request.TransactionId);
        return new PaymentResult(false, request.TransactionId, "Insufficient funds");
    }
}

/// <summary>
/// Fake PayPal payment gateway implementation.
/// </summary>
public class FakePayPalGateway : IPaymentGateway
{
    private readonly ILogger<FakePayPalGateway> _logger;
    private static readonly Random _random = new(43); // Different seed for variety
    
    public FakePayPalGateway(ILogger<FakePayPalGateway> logger)
    {
        _logger = logger;
    }
    
    public string ProviderName => "PayPal";
    
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        // Simulate network delay
        await Task.Delay(80, cancellationToken);
        
        // Simulate 90% success rate
        var success = _random.NextDouble() > 0.10;
        
        if (success)
        {
            _logger.LogInformation("PayPal payment processed: {TransactionId}, Amount: {Amount} {Currency}",
                request.TransactionId, request.Amount, request.Currency);
            return new PaymentResult(true, request.TransactionId);
        }
        
        _logger.LogWarning("PayPal payment failed: {TransactionId}", request.TransactionId);
        return new PaymentResult(false, request.TransactionId, "Payment declined");
    }
}
