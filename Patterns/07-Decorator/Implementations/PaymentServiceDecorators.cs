using DesignPatterns.Playground.Api.Infrastructure;
using DesignPatterns.Playground.Api.Patterns._07_Decorator.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._07_Decorator.Implementations;

/// <summary>
/// Logging decorator for payment service.
/// Decorator pattern: adds logging without modifying core service.
/// </summary>
public class LoggingPaymentServiceDecorator : IPaymentService
{
    private readonly IPaymentService _inner;
    private readonly ILogger<LoggingPaymentServiceDecorator> _logger;
    
    public LoggingPaymentServiceDecorator(
        IPaymentService inner,
        ILogger<LoggingPaymentServiceDecorator> logger)
    {
        _inner = inner;
        _logger = logger;
    }
    
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Payment request started: {TransactionId}, Amount: {Amount} {Currency}",
            request.TransactionId, request.Amount, request.Currency);
        
        var result = await _inner.ProcessPaymentAsync(request, cancellationToken);
        
        _logger.LogInformation("Payment request completed: {TransactionId}, Success: {Success}",
            request.TransactionId, result.Success);
        
        return result;
    }
}

/// <summary>
/// Metrics decorator for payment service.
/// </summary>
public class MetricsPaymentServiceDecorator : IPaymentService
{
    private readonly IPaymentService _inner;
    private readonly IMetrics _metrics;
    
    public MetricsPaymentServiceDecorator(
        IPaymentService inner,
        IMetrics metrics)
    {
        _inner = inner;
        _metrics = metrics;
    }
    
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;
        
        try
        {
            var result = await _inner.ProcessPaymentAsync(request, cancellationToken);
            
            var duration = DateTime.UtcNow - startTime;
            _metrics.RecordDuration("payment.process.duration", duration, new Dictionary<string, string>
            {
                ["success"] = result.Success.ToString().ToLower(),
                ["currency"] = request.Currency
            });
            
            _metrics.IncrementCounter("payment.process.count", new Dictionary<string, string>
            {
                ["success"] = result.Success.ToString().ToLower()
            });
            
            return result;
        }
        catch (Exception)
        {
            var duration = DateTime.UtcNow - startTime;
            _metrics.RecordDuration("payment.process.duration", duration, new Dictionary<string, string>
            {
                ["success"] = "false",
                ["error"] = "exception"
            });
            _metrics.IncrementCounter("payment.process.count", new Dictionary<string, string>
            {
                ["success"] = "false"
            });
            throw;
        }
    }
}

/// <summary>
/// Retry decorator for payment service.
/// </summary>
public class RetryPaymentServiceDecorator : IPaymentService
{
    private readonly IPaymentService _inner;
    private readonly ILogger<RetryPaymentServiceDecorator> _logger;
    private readonly int _maxRetries;
    
    public RetryPaymentServiceDecorator(
        IPaymentService inner,
        ILogger<RetryPaymentServiceDecorator> logger,
        int maxRetries = 3)
    {
        _inner = inner;
        _logger = logger;
        _maxRetries = maxRetries;
    }
    
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        var attempt = 0;
        
        while (attempt < _maxRetries)
        {
            try
            {
                var result = await _inner.ProcessPaymentAsync(request, cancellationToken);
                
                if (result.Success || attempt >= _maxRetries - 1)
                    return result;
                
                attempt++;
                _logger.LogWarning("Payment failed, retrying ({Attempt}/{MaxRetries}): {TransactionId}",
                    attempt, _maxRetries, request.TransactionId);
                
                await Task.Delay(100 * attempt, cancellationToken); // Exponential backoff
            }
            catch (Exception ex)
            {
                attempt++;
                if (attempt >= _maxRetries)
                {
                    _logger.LogError(ex, "Payment failed after {MaxRetries} attempts: {TransactionId}",
                        _maxRetries, request.TransactionId);
                    return new PaymentResult(false, request.TransactionId, ex.Message);
                }
                
                await Task.Delay(100 * attempt, cancellationToken);
            }
        }
        
        return new PaymentResult(false, request.TransactionId, "Max retries exceeded");
    }
}
