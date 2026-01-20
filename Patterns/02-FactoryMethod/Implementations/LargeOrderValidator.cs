using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Implementations;

/// <summary>
/// Large order validator with additional checks for high-value orders.
/// </summary>
public class LargeOrderValidator : IOrderValidator
{
    private const decimal LargeOrderThreshold = 100000m;
    
    public string ValidatorType => "LargeOrder";
    
    public ValidationResult Validate(Order order)
    {
        var errors = new List<string>();
        var orderValue = order.Quantity * order.Price;
        
        // Standard validations
        if (order.Quantity <= 0)
            errors.Add("Quantity must be greater than zero");
        
        if (order.Price <= 0)
            errors.Add("Price must be greater than zero");
        
        if (string.IsNullOrWhiteSpace(order.Symbol))
            errors.Add("Symbol is required");
        
        // Large order specific validations
        if (orderValue > LargeOrderThreshold)
        {
            if (orderValue > LargeOrderThreshold * 10)
                errors.Add("Order value exceeds maximum allowed (10x threshold)");
        }
        
        return new ValidationResult(errors.Count == 0, errors);
    }
}
