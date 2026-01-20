using DesignPatterns.Playground.Api.Domain;

namespace DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Contracts;

/// <summary>
/// Factory interface for creating order validators.
/// Factory Method pattern: encapsulates validator creation logic.
/// </summary>
public interface IOrderValidatorFactory
{
    /// <summary>
    /// Create a validator based on order characteristics.
    /// </summary>
    IOrderValidator CreateValidator(Order order);
}
