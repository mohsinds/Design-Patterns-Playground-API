using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._14_Prototype.Contracts;
using DesignPatterns.Playground.Api.Patterns._14_Prototype.Implementations;

namespace DesignPatterns.Playground.Api.Patterns._14_Prototype.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Prototype pattern.
/// </summary>
public class PrototypeScenario
{
    private readonly ILogger<PrototypeScenario> _logger;
    
    public PrototypeScenario(ILogger<PrototypeScenario> logger)
    {
        _logger = logger;
    }
    
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Create original order
        var originalOrder = new Order(
            OrderId: "ORD-PROTO-001",
            AccountId: "ACC-001",
            Symbol: "AAPL",
            Side: OrderSide.Buy,
            Quantity: 100,
            Price: 150m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var originalSnapshot = new OrderSnapshot(originalOrder, new Dictionary<string, object>
        {
            ["BacktestId"] = "BT-001",
            ["Strategy"] = "Momentum"
        });
        
        results.Add(new
        {
            Action = "Original Snapshot",
            OrderId = originalSnapshot.Order.OrderId,
            SnapshotTimestamp = originalSnapshot.SnapshotTimestamp,
            Metadata = originalSnapshot.Metadata
        });
        
        // Clone snapshot for backtest
        var clonedSnapshot = originalSnapshot.Clone();
        clonedSnapshot.Metadata["BacktestId"] = "BT-002"; // Modify clone
        
        results.Add(new
        {
            Action = "Cloned Snapshot",
            OrderId = clonedSnapshot.Order.OrderId,
            SnapshotTimestamp = clonedSnapshot.SnapshotTimestamp,
            Metadata = clonedSnapshot.Metadata,
            Note = "Clone is independent - modifying clone doesn't affect original"
        });
        
        // Portfolio snapshot clone
        var portfolioSnapshot = new PortfolioSnapshot(
            orders: new List<OrderSnapshot> { originalSnapshot, clonedSnapshot },
            positions: new Dictionary<string, decimal> { ["AAPL"] = 200 },
            cashBalance: 10000m
        );
        
        var clonedPortfolio = portfolioSnapshot.Clone();
        clonedPortfolio.Positions["AAPL"] = 300; // Modify clone
        
        results.Add(new
        {
            Action = "Portfolio Clone",
            OriginalPositions = portfolioSnapshot.Positions,
            ClonedPositions = clonedPortfolio.Positions,
            Note = "Portfolio clone is independent"
        });
        
        return new PatternDemoResponse(
            Pattern: "Prototype",
            Description: "Demonstrates prototype pattern: creates deep copies of objects for snapshots, backtests, and cloning scenarios.",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["UseCase"] = "Backtesting, snapshots, cloning",
                ["DeepCopy"] = true,
                ["Independence"] = "Clones are independent of originals"
            }
        );
    }
    
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Test 1: Clone creates independent copy
        var originalOrder = new Order(
            OrderId: "ORD-TEST",
            AccountId: "ACC-TEST",
            Symbol: "TEST",
            Side: OrderSide.Buy,
            Quantity: 10,
            Price: 100m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var originalSnapshot = new OrderSnapshot(originalOrder);
        var clonedSnapshot = originalSnapshot.Clone();
        
        checks.Add(new TestCheck(
            "Clone Creates Copy",
            clonedSnapshot != null && clonedSnapshot.Order.OrderId == originalSnapshot.Order.OrderId,
            $"Cloned snapshot with order {clonedSnapshot.Order.OrderId}"
        ));
        
        // Test 2: Clone is independent (modifying clone doesn't affect original)
        clonedSnapshot.Metadata["Test"] = "Modified";
        var originalHasTest = originalSnapshot.Metadata.ContainsKey("Test");
        
        checks.Add(new TestCheck(
            "Clone Independence",
            !originalHasTest,
            "Modifying clone doesn't affect original"
        ));
        
        // Test 3: Portfolio clone works
        var portfolio = new PortfolioSnapshot(
            orders: new List<OrderSnapshot> { originalSnapshot },
            positions: new Dictionary<string, decimal> { ["TEST"] = 10 },
            cashBalance: 1000m
        );
        
        var clonedPortfolio = portfolio.Clone();
        clonedPortfolio.Positions["TEST"] = 20;
        
        checks.Add(new TestCheck(
            "Portfolio Clone",
            portfolio.Positions["TEST"] == 10 && clonedPortfolio.Positions["TEST"] == 20,
            "Portfolio clone is independent"
        ));
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Prototype",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
