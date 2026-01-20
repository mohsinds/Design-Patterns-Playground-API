using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._14_Prototype.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._14_Prototype.Implementations;

/// <summary>
/// Order snapshot for backtesting/cloning.
/// Prototype pattern: enables creating copies for backtests, snapshots, etc.
/// </summary>
public class OrderSnapshot : IPrototype<OrderSnapshot>
{
    public Order Order { get; }
    public DateTime SnapshotTimestamp { get; }
    public Dictionary<string, object> Metadata { get; }
    
    public OrderSnapshot(Order order, Dictionary<string, object>? metadata = null)
    {
        Order = order;
        SnapshotTimestamp = DateTime.UtcNow;
        Metadata = metadata ?? new Dictionary<string, object>();
    }
    
    /// <summary>
    /// Clone the snapshot (deep copy).
    /// Prototype pattern: creates independent copy for backtesting.
    /// </summary>
    public OrderSnapshot Clone()
    {
        // Deep copy order
        var clonedOrder = new Order(
            OrderId: Order.OrderId,
            AccountId: Order.AccountId,
            Symbol: Order.Symbol,
            Side: Order.Side,
            Quantity: Order.Quantity,
            Price: Order.Price,
            Status: Order.Status,
            CreatedAt: Order.CreatedAt,
            UpdatedAt: Order.UpdatedAt,
            RowVersion: Order.RowVersion
        );
        
        // Deep copy metadata
        var clonedMetadata = new Dictionary<string, object>(Metadata);
        
        return new OrderSnapshot(clonedOrder, clonedMetadata);
    }
}

/// <summary>
/// Portfolio snapshot for backtesting.
/// </summary>
public class PortfolioSnapshot : IPrototype<PortfolioSnapshot>
{
    public List<OrderSnapshot> Orders { get; }
    public Dictionary<string, decimal> Positions { get; }
    public decimal CashBalance { get; }
    public DateTime SnapshotTimestamp { get; }
    
    public PortfolioSnapshot(
        List<OrderSnapshot> orders,
        Dictionary<string, decimal> positions,
        decimal cashBalance)
    {
        Orders = orders;
        Positions = positions;
        CashBalance = cashBalance;
        SnapshotTimestamp = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Clone the portfolio snapshot (deep copy).
    /// </summary>
    public PortfolioSnapshot Clone()
    {
        var clonedOrders = Orders.Select(o => o.Clone()).ToList();
        var clonedPositions = new Dictionary<string, decimal>(Positions);
        
        return new PortfolioSnapshot(clonedOrders, clonedPositions, CashBalance);
    }
}
