using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._04_Builder.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._04_Builder.Implementations;

/// <summary>
/// Builder implementation for constructing orders.
/// Builder pattern: provides fluent interface for constructing complex Order objects.
/// Thread-safety: Not thread-safe; builder instances should not be shared across threads.
/// </summary>
public class OrderBuilder : IOrderBuilder
{
    private string? _accountId;
    private string? _symbol;
    private OrderSide? _side;
    private decimal? _quantity;
    private decimal? _price;
    private decimal? _limitPrice;
    
    public IOrderBuilder WithAccount(string accountId)
    {
        _accountId = accountId;
        return this;
    }
    
    public IOrderBuilder WithSymbol(string symbol)
    {
        _symbol = symbol;
        return this;
    }
    
    public IOrderBuilder WithSide(OrderSide side)
    {
        _side = side;
        return this;
    }
    
    public IOrderBuilder WithQuantity(decimal quantity)
    {
        _quantity = quantity;
        return this;
    }
    
    public IOrderBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }
    
    public IOrderBuilder WithLimitPrice(decimal? limitPrice)
    {
        _limitPrice = limitPrice;
        return this;
    }
    
    public Order Build()
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(_accountId))
            throw new InvalidOperationException("Account ID is required");
        if (string.IsNullOrWhiteSpace(_symbol))
            throw new InvalidOperationException("Symbol is required");
        if (_side == null)
            throw new InvalidOperationException("Side is required");
        if (_quantity == null || _quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero");
        if (_price == null || _price <= 0)
            throw new InvalidOperationException("Price must be greater than zero");
        
        var orderId = $"ORD-{Guid.NewGuid():N}";
        var now = DateTime.UtcNow;
        
        return new Order(
            OrderId: orderId,
            AccountId: _accountId!,
            Symbol: _symbol!,
            Side: _side.Value,
            Quantity: _quantity.Value,
            Price: _price.Value,
            Status: OrderStatus.Pending,
            CreatedAt: now,
            UpdatedAt: null,
            RowVersion: 0
        );
    }
    
    public IOrderBuilder Reset()
    {
        _accountId = null;
        _symbol = null;
        _side = null;
        _quantity = null;
        _price = null;
        _limitPrice = null;
        return this;
    }
}
