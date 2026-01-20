using DesignPatterns.Playground.Api.Infrastructure;
using DesignPatterns.Playground.Api.Patterns._07_Decorator.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._07_Decorator.Implementations;

/// <summary>
/// Core payment service implementation.
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly IPaymentGateway _gateway;
    private readonly ILogger<PaymentService> _logger;
    
    public PaymentService(
        IPaymentGateway gateway,
        ILogger<PaymentService> logger)
    {
        _gateway = gateway;
        _logger = logger;
    }
    
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing payment {TransactionId}", request.TransactionId);
        return await _gateway.ProcessPaymentAsync(request, cancellationToken);
    }
}
