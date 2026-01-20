using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Implementations;

/// <summary>
/// Standard order validator for regular orders.
/// </summary>
public class StandardOrderValidator : IOrderValidator
{
    public string ValidatorType => "Standard";
    
    public ValidationResult Validate(Order order)
    {
        var errors = new List<string>();
        
        if (order.Quantity <= 0)
            errors.Add("Quantity must be greater than zero");
        
        if (order.Price <= 0)
            errors.Add("Price must be greater than zero");
        
        if (string.IsNullOrWhiteSpace(order.Symbol))
            errors.Add("Symbol is required");
        
        return new ValidationResult(errors.Count == 0, errors);
    }
}
