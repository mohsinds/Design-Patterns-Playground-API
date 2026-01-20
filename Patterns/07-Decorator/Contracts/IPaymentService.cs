using DesignPatterns.Playground.Api.Infrastructure;

namespace DesignPatterns.Playground.Api.Patterns._07_Decorator.Contracts;

/// <summary>
/// Payment service interface.
/// Decorator pattern: allows adding cross-cutting concerns (logging, metrics, retries) dynamically.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Process a payment.
    /// </summary>
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default);
}
