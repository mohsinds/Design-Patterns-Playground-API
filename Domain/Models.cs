namespace DesignPatterns.Playground.Api.Domain;

/// <summary>
/// Represents a trading order in the Order Management System (OMS).
/// </summary>
public record Order(
    string OrderId,
    string AccountId,
    string Symbol,
    OrderSide Side,
    decimal Quantity,
    decimal Price,
    OrderStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt = null,
    long RowVersion = 0
);

/// <summary>
/// Order side: Buy or Sell.
/// </summary>
public enum OrderSide
{
    Buy,
    Sell
}

/// <summary>
/// Order status in the lifecycle state machine.
/// </summary>
public enum OrderStatus
{
    Pending,
    Validated,
    Placed,
    PartiallyFilled,
    Filled,
    Cancelled,
    Rejected
}

/// <summary>
/// Represents a trading account with balance information.
/// </summary>
public record Account(
    string AccountId,
    string AccountName,
    decimal Balance,
    string Currency,
    DateTime CreatedAt
);

/// <summary>
/// Immutable money value with currency.
/// </summary>
public record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency) => new(0, currency);
    
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException($"Cannot add {Currency} and {other.Currency}");
        return new Money(Amount + other.Amount, Currency);
    }
    
    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException($"Cannot subtract {Currency} and {other.Currency}");
        return new Money(Amount - other.Amount, Currency);
    }
}

/// <summary>
/// Ledger entry for accounting/tracking financial transactions.
/// </summary>
public record LedgerEntry(
    string EntryId,
    string AccountId,
    Money Amount,
    string Description,
    DateTime Timestamp,
    string TransactionId
);

/// <summary>
/// Market quote for a symbol.
/// </summary>
public record Quote(
    string Symbol,
    decimal Bid,
    decimal Ask,
    decimal Last,
    DateTime Timestamp
);

/// <summary>
/// Request to place an order.
/// </summary>
public record PlaceOrderRequest(
    string AccountId,
    string Symbol,
    OrderSide Side,
    decimal Quantity,
    decimal? LimitPrice = null
);

/// <summary>
/// Request to cancel an order.
/// </summary>
public record CancelOrderRequest(string OrderId, string AccountId);

/// <summary>
/// Request to replace an order.
/// </summary>
public record ReplaceOrderRequest(
    string OrderId,
    string AccountId,
    decimal? NewQuantity = null,
    decimal? NewPrice = null
);
