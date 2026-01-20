using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Implementations;

/// <summary>
/// Factory for creating order validators based on order characteristics.
/// Factory Method pattern: encapsulates the creation logic, allowing easy extension.
/// 
/// HOW IT WORKS:
/// =============
/// 1. CreateValidator() receives an Order object
/// 2. Calculates order value (Quantity * Price)
/// 3. Compares against LargeOrderThreshold (100,000)
/// 4. If orderValue >= threshold: creates LargeOrderValidator (has additional risk checks)
/// 5. Otherwise: creates StandardOrderValidator (basic validation)
/// 6. Returns IOrderValidator interface (polymorphism - caller doesn't know concrete type)
/// 
/// EXTENSIBILITY:
/// - To add new validator type (e.g., HighFrequencyOrderValidator):
///   1. Create new validator class implementing IOrderValidator
///   2. Add condition in CreateValidator() method
///   3. No changes needed in code that uses validators
/// 
/// Thread-safety: Stateless factory, safe for concurrent use.
/// </summary>
public class OrderValidatorFactory : IOrderValidatorFactory
{
    private const decimal LargeOrderThreshold = 100000m;
    
    /// <summary>
    /// Create appropriate validator based on order value and characteristics.
    /// Thread-safety: Stateless factory, safe for concurrent use.
    /// </summary>
    public IOrderValidator CreateValidator(Order order)
    {
        var orderValue = order.Quantity * order.Price;
        
        // Factory method: decide which validator to create
        if (orderValue >= LargeOrderThreshold)
        {
            return new LargeOrderValidator();
        }
        
        return new StandardOrderValidator();
    }
}
