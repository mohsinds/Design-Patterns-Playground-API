using DesignPatterns.Playground.Api.Domain;

namespace DesignPatterns.Playground.Api.Patterns._15_ChainOfResponsibility.Contracts;

/// <summary>
/// Validation handler interface.
/// Chain of Responsibility pattern: validation pipeline where each handler can process or pass to next.
/// </summary>
public interface IValidationHandler
{
    /// <summary>
    /// Set next handler in chain.
    /// </summary>
    IValidationHandler SetNext(IValidationHandler handler);
    
    /// <summary>
    /// Handle validation.
    /// </summary>
    Task<ValidationResult> HandleAsync(Order order, CancellationToken cancellationToken = default);
}

/// <summary>
/// Validation result.
/// </summary>
public record ValidationResult(bool IsValid, List<string> Errors);
