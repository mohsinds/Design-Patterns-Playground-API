using DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Contracts;

namespace DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Implementations;

/// <summary>
/// Payment service implementation.
/// Strategy pattern: Delegates payment processing to the appropriate provider strategy selected at runtime.
/// </summary>
public sealed class PaymentService : IPaymentService
{
    private readonly IPaymentProviderResolver _resolver;
    private readonly ILogger<PaymentService> _logger;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentService"/> class.
    /// </summary>
    /// <param name="resolver">The payment provider resolver for selecting providers.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when resolver or logger is null.</exception>
    public PaymentService(
        IPaymentProviderResolver resolver,
        ILogger<PaymentService> logger)
    {
        ArgumentNullException.ThrowIfNull(resolver);
        ArgumentNullException.ThrowIfNull(logger);
        
        _resolver = resolver;
        _logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<PaymentResult> ProcessPaymentAsync(
        PaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        _logger.LogInformation("Processing payment request: Provider={Provider}, Amount={Amount}, Currency={Currency}",
            request.ProviderKey, request.Amount, request.Currency);
        
        // Resolve provider using strategy pattern
        var provider = _resolver.ResolveProvider(request.ProviderKey);
        
        if (provider == null)
        {
            var availableProviders = string.Join(", ", _resolver.GetAvailableProviders().Select(p => p.ProviderKey));
            throw new InvalidOperationException(
                $"Payment provider '{request.ProviderKey}' not found. Available providers: {availableProviders}");
        }
        
        // Validate payment using provider's validation strategy
        if (!provider.ValidatePayment(request.Amount, request.Currency))
        {
            _logger.LogWarning("Payment validation failed for provider '{ProviderKey}'",
                request.ProviderKey);
            throw new InvalidOperationException(
                $"Payment validation failed for provider '{request.ProviderKey}'");
        }
        
        // Process payment using provider's processing strategy
        var result = await provider.ProcessPaymentAsync(
            request.Amount,
            request.Currency,
            request.CustomerEmail,
            cancellationToken);
        
        _logger.LogInformation("Payment processed: TransactionId={TransactionId}, Status={Status}",
            result.TransactionId, result.Status);
        
        return result;
    }
    
    /// <inheritdoc />
    public IEnumerable<ProviderInfo> GetAvailableProviders() =>
        _resolver.GetAvailableProviders().Select(p => new ProviderInfo
        {
            Key = p.ProviderKey,
            MinimumAmount = p.MinimumAmount,
            SupportedCurrencies = p.SupportedCurrencies
        });
}
