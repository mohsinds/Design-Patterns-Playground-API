using DesignPatterns.Playground.Api.Domain;

namespace DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Contracts;

/// <summary>
/// Order validator interface.
/// Factory Method pattern: different validators created based on order type/context.
/// </summary>
public interface IOrderValidator
{
    /// <summary>
    /// Validate an order.
    /// </summary>
    ValidationResult Validate(Order order);
    
    /// <summary>
    /// Get validator type name.
    /// </summary>
    string ValidatorType { get; }
}

/// <summary>
/// Validation result.
/// </summary>
public record ValidationResult(bool IsValid, List<string> Errors);
