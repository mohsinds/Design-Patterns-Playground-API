using DesignPatterns.Playground.Api.Domain;

namespace DesignPatterns.Playground.Api.Patterns._04_Builder.Contracts;

/// <summary>
/// Builder interface for constructing orders.
/// Builder pattern: allows step-by-step construction of complex objects.
/// </summary>
public interface IOrderBuilder
{
    /// <summary>
    /// Set the account ID.
    /// </summary>
    IOrderBuilder WithAccount(string accountId);
    
    /// <summary>
    /// Set the symbol.
    /// </summary>
    IOrderBuilder WithSymbol(string symbol);
    
    /// <summary>
    /// Set the side (Buy/Sell).
    /// </summary>
    IOrderBuilder WithSide(OrderSide side);
    
    /// <summary>
    /// Set the quantity.
    /// </summary>
    IOrderBuilder WithQuantity(decimal quantity);
    
    /// <summary>
    /// Set the price.
    /// </summary>
    IOrderBuilder WithPrice(decimal price);
    
    /// <summary>
    /// Set optional limit price.
    /// </summary>
    IOrderBuilder WithLimitPrice(decimal? limitPrice);
    
    /// <summary>
    /// Build the order.
    /// </summary>
    Order Build();
    
    /// <summary>
    /// Reset the builder.
    /// </summary>
    IOrderBuilder Reset();
}
