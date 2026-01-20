using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._11_Repository.Contracts;
using DesignPatterns.Playground.Api.Patterns._11_Repository.Implementations;

namespace DesignPatterns.Playground.Api.Patterns._11_Repository.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Repository pattern.
/// </summary>
public class RepositoryScenario
{
    private readonly IRepository<Order, string> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RepositoryScenario> _logger;
    
    public RepositoryScenario(
        IRepository<Order, string> orderRepository,
        IUnitOfWork unitOfWork,
        ILogger<RepositoryScenario> logger)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Create and save order
        var order = new Order(
            OrderId: "ORD-REPO-001",
            AccountId: "ACC-001",
            Symbol: "AAPL",
            Side: OrderSide.Buy,
            Quantity: 100,
            Price: 150m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        _orderRepository.AddAsync(order).Wait();
        results.Add(new
        {
            Action = "Add Order",
            Order = order
        });
        
        // Retrieve order
        var retrieved = _orderRepository.GetByIdAsync(order.OrderId).Result;
        results.Add(new
        {
            Action = "Retrieve Order",
            Found = retrieved != null,
            OrderId = retrieved?.OrderId
        });
        
        // Update order
        if (retrieved != null)
        {
            var updated = retrieved with { Status = OrderStatus.Placed };
            _orderRepository.UpdateAsync(updated).Wait();
            results.Add(new
            {
                Action = "Update Order",
                OrderId = updated.OrderId,
                NewStatus = updated.Status
            });
        }
        
        // Unit of Work demo
        _unitOfWork.BeginTransactionAsync().Wait();
        var order2 = new Order(
            OrderId: "ORD-REPO-002",
            AccountId: "ACC-001",
            Symbol: "MSFT",
            Side: OrderSide.Sell,
            Quantity: 50,
            Price: 300m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        _orderRepository.AddAsync(order2).Wait();
        _unitOfWork.SaveChangesAsync().Wait();
        _unitOfWork.CommitAsync().Wait();
        
        results.Add(new
        {
            Action = "Unit of Work",
            Description = "Multiple operations in transaction",
            OrderId = order2.OrderId
        });
        
        return new PatternDemoResponse(
            Pattern: "Repository",
            Description: "Demonstrates repository pattern: abstracts data access, enables testing. Includes Unit of Work for transaction coordination.",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["Abstraction"] = "Data access abstracted from business logic",
                ["Testability"] = "Easy to mock or use in-memory implementation",
                ["UnitOfWork"] = "Coordinates multiple repository operations in transactions"
            }
        );
    }
    
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Test 1: Repository can add and retrieve
        var order = new Order(
            OrderId: "ORD-TEST-001",
            AccountId: "ACC-TEST",
            Symbol: "TEST",
            Side: OrderSide.Buy,
            Quantity: 10,
            Price: 100m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        _orderRepository.AddAsync(order).Wait();
        var retrieved = _orderRepository.GetByIdAsync(order.OrderId).Result;
        
        checks.Add(new TestCheck(
            "Repository Add and Retrieve",
            retrieved != null && retrieved.OrderId == order.OrderId,
            $"Retrieved order {retrieved?.OrderId}"
        ));
        
        // Test 2: Repository can update
        var updated = order with { Status = OrderStatus.Placed };
        _orderRepository.UpdateAsync(updated).Wait();
        var updatedRetrieved = _orderRepository.GetByIdAsync(order.OrderId).Result;
        
        checks.Add(new TestCheck(
            "Repository Update",
            updatedRetrieved != null && updatedRetrieved.Status == OrderStatus.Placed,
            $"Updated order status to {updatedRetrieved?.Status}"
        ));
        
        // Test 3: Repository can delete
        _orderRepository.DeleteAsync(order.OrderId).Wait();
        var deleted = _orderRepository.GetByIdAsync(order.OrderId).Result;
        
        checks.Add(new TestCheck(
            "Repository Delete",
            deleted == null,
            "Order was deleted"
        ));
        
        // Test 4: Unit of Work works
        _unitOfWork.BeginTransactionAsync().Wait();
        var order2 = new Order(
            OrderId: "ORD-TEST-002",
            AccountId: "ACC-TEST",
            Symbol: "TEST",
            Side: OrderSide.Buy,
            Quantity: 10,
            Price: 100m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        _orderRepository.AddAsync(order2).Wait();
        _unitOfWork.SaveChangesAsync().Wait();
        _unitOfWork.CommitAsync().Wait();
        
        var uowRetrieved = _orderRepository.GetByIdAsync(order2.OrderId).Result;
        checks.Add(new TestCheck(
            "Unit of Work",
            uowRetrieved != null,
            $"Unit of Work saved order {uowRetrieved?.OrderId}"
        ));
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Repository",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
